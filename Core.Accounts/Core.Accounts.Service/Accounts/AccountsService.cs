using AutoMapper;
using Common.Constants;
using Common.Extensions;
using Common.Model.Errors;
using Common.ServiceBus.Interfaces;
using Common.Services;
using Core.Accounts.DAL;
using Core.Accounts.DAL.Constants;
using Core.Accounts.DAL.Entity;
using Core.Accounts.Infrastructure.HttpClients;
using Core.Accounts.Service.Accounts.Command;
using Core.Accounts.Service.Accounts.Extensions;
using Core.Accounts.Service.Accounts.Query.Request;
using Core.Accounts.Service.Accounts.Query.Response;
using Core.Accounts.Service.Common;
using Core.Accounts.Service.Interface;
using HashidsNet;
using Localization.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Core.Accounts.Service.Accounts
{
    public sealed class AccountsService : BaseService<AccountsDbContext, Account, AccountsResponse, AccountsBaseResponse, AccountsQuery>, IAccountsService
    {

        private readonly RegisterAccountCommandValidator _registerAccountCmdValidator;
        private readonly UpdateAccountCommandValidator _updateAccountCmdValidator;
        private readonly EmailVerificationValidator _emailVerificationValidator;
        private readonly ChangePasswordValidator _changePasswordValidator;
        private readonly CreateAccountRolesCommandValidator _createAccountRolesValidator;
        private readonly AssignAccountTenantRolesCommandValidator _assignAccountToTenantValidator;
        private readonly ITenantsApi _tenantsApi;
        private readonly IServiceBusSender _serviceBusSender;
        private readonly AuthValidations _authValidations;
        private readonly IRolesService _rolesService;

        public AccountsService(AccountsDbContext ctx,
                           IMapper mapper,
                           IHashids hashids,
                           RegisterAccountCommandValidator registerAccountCommandValidator,
                           UpdateAccountCommandValidator updateAccountCmdValidator,
                           EmailVerificationValidator emailVerificationValidator,
                           CreateAccountRolesCommandValidator createAccountRolesValidator,
                           AssignAccountTenantRolesCommandValidator assignAccountToTenantValidator,
                           ITenantsApi tenantsApi,
                           IStringLocalizer<SharedResource> stringLocalizer,
                           IServiceBusSender serviceBusSender,
                           ChangePasswordValidator changePasswordValidator,
                           AuthValidations authValidations,
                           IRolesService rolesService)
             : base(ctx, mapper, stringLocalizer, hashids)
        {
            _registerAccountCmdValidator = registerAccountCommandValidator;
            _updateAccountCmdValidator = updateAccountCmdValidator;
            _createAccountRolesValidator = createAccountRolesValidator;
            _emailVerificationValidator = emailVerificationValidator;
            _assignAccountToTenantValidator = assignAccountToTenantValidator;
            _tenantsApi = tenantsApi;
            _serviceBusSender = serviceBusSender;
            _changePasswordValidator = changePasswordValidator;
            _authValidations = authValidations;
            _rolesService = rolesService;
        }

        public async Task<Account> Create(RegisterAccountCommand cmd)
        {
            _registerAccountCmdValidator.ValidateCmd(cmd);
            Account account = null;

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                account = new Account
                {
                    FirstName = cmd.FirstName,
                    LastName = cmd.LastName,
                    Email = cmd.Email,
                    PhoneNumber = cmd.PhoneNumber,
                    Status = AccountVerificationStatus.Verified,
                    CreatedById = _ctx.CurrentAccount.Id,
                    UpdatedById = _ctx.CurrentAccount.Id,
                }
                 .SetPassword(cmd.Password)
                 .GenerateVerificationToken();

                var result = await _ctx.Accounts.AddAsync(account);

                await _ctx.SaveChangesAsync();
                scope.Complete();
            }

            return account;
        }

        public async Task<Account> Update(int id, UpdateAccountCommand cmd)
        {
            _updateAccountCmdValidator.ValidateCmd(cmd);

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                var account = await GetQueryable(Includes()).FirstOrDefaultAsync(x => x.Id == id);

                if (account != null)
                {
                    account.FirstName = account.FirstName == cmd.FirstName ? account.FirstName : cmd.FirstName;
                    account.LastName = account.LastName == cmd.LastName ? account.LastName : cmd.LastName;
                    account.PhoneNumber = account.PhoneNumber == cmd.PhoneNumber ? account.PhoneNumber : cmd.PhoneNumber;
                    account.UpdatedById = _ctx.CurrentAccount.Id;
                    await _ctx.SaveChangesAsync();
                }
                scope.Complete();

                return account;
            }
        }

        public async Task<bool> AssignAccountTenantRoles(AssignAccountTenantRolesCommand cmd)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                _assignAccountToTenantValidator.Validate(cmd);

                var tenantRoles = await _rolesService.GetRolesByTenantId(cmd.TenantId);

                var roles = _mapper.Map<List<Role>>(tenantRoles);

                var initialRoles = new List<string> { _hashids.Encode(roles.First(r => r.Name == RoleConstants.ClientRole).Id) };

                if (!cmd.IsExistingTenant)
                {
                    initialRoles.Add(_hashids.Encode(roles.First(r => r.Name == RoleConstants.AdminRole).Id));
                }

                var account = await CreateAccountRoles(new CreateAccountRolesCommand { AccountId = cmd.AccountId, RoleIds = initialRoles });
                scope.Complete();

                return account == null ? false : true;
            }
        }

        public async Task<Account> CreateAccountRoles(CreateAccountRolesCommand cmd)
        {
            _createAccountRolesValidator.ValidateCmd(cmd);

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                var account = await GetQueryable(Includes()).FirstAsync(x => x.Id == _hashids.DecodeSingle(cmd.AccountId));

                var accountRolles = cmd.RoleIds.Select(x => new AccountRole() { AccountId = _hashids.DecodeSingle(cmd.AccountId), RoleId = _hashids.DecodeSingle(x) }).ToList();

                _ctx.AccountRoles.AddRange(accountRolles);

                await _ctx.SaveChangesAsync();

                scope.Complete();

                return account;
            }
        }

        public override async Task<bool> Delete(int id)
        {
            await CheckIfDeleteAccountIsSuperAdmin(id);

            return await base.Delete(id);
        }

        public async Task<bool> QuickValidation(string field, string value)
        {
            if (string.IsNullOrEmpty(field) || string.IsNullOrEmpty(value))
                throw new ValidationError(new List<ApiError>() { new ApiError(400, _stringLocalizer["BadParams"]) });

            if (field.ToLower().Trim().Contains("email"))
            {
                return await _authValidations.AccountWithEmailNotExistsAsync(value);
            }

            //if (field.ToLower().Trim().Contains("phone"))
            //{
            //    return await _authValidations.AccountWithPhoneNumberNotExistsAsync(value);
            //}

            return false;
        }

        public async Task<bool> EmailVerification(EmailVerificationCommand cmd)
        {
            _emailVerificationValidator.ValidateCmd(cmd);

            var Account = await GetQueryable(Includes()).FirstOrDefaultAsync(x => x.Email == cmd.Email);

            if (Account == default) return false;

            Account.Status = AccountVerificationStatus.Verified;
            Account.VerificationExp = System.DateTime.Now;

            Account.SetPassword(cmd.Password);

            if (string.IsNullOrEmpty(Account.Password))
                throw new ValidationError(new List<ApiError>() { new ApiError(400, _stringLocalizer["CannotValidateAccountWithoutPassword"]) });

            Account.AccountRoles = new List<AccountRole> { new AccountRole() { AccountId = Account.Id, RoleId = _ctx.Roles.FirstOrDefaultAsync<Role>(r => r.Name == RoleConstants.AdminRole).Id } };
            await _ctx.SaveChangesAsync();

            return true;
        }

        public async Task<string> ResendEmailVerification(string email)
        {
            var Account = await GetQueryable(Includes()).FirstOrDefaultAsync(x => x.Email == email);

            if (Account == default) return string.Empty;

            Account.Status = AccountVerificationStatus.NotVerified;
            Account.GenerateVerificationToken();

            await _ctx.SaveChangesAsync();

            // TODO extract in method
            //if (Account != null)
            //{
            //    var AccountServiceBusMessage = _mapper.Map<AccountServiceBusMessageObject>(Account);
            //    AccountServiceBusMessage.NotificationEnum = NotificationEnum.AccounVerificationTokenCreated;
            //    await _serviceBusSender.SendServiceBusMessages(new List<AccountServiceBusMessageObject>() { AccountServiceBusMessage });
            //}

            return Account.VerificationToken;
        }

        public async Task<string> ResetPassword(string email)
        {
            var Account = await GetQueryable(Includes()).FirstOrDefaultAsync(x => x.Email == email);

            if (Account == default) return string.Empty;

            Account.GenerateResetToken();
            Account.Status = AccountVerificationStatus.PasswordResetRequested;

            // TODO EMIT Account with RESET Token - from Service

            await _ctx.SaveChangesAsync();

            return Account.ResetToken;
        }

        public async Task<bool> ChangePassword(ChangePasswordCommand cmd)
        {
            _changePasswordValidator.ValidateCmd(cmd);

            var Account = await GetQueryable(Includes()).FirstOrDefaultAsync(x => x.Email == cmd.Email);

            if (Account == default) return false;

            Account.SetPassword(cmd.Password);

            Account.Status = AccountVerificationStatus.PasswordResetRequested;
            Account.ResetExp = System.DateTime.Now;

            await _ctx.SaveChangesAsync();

            return true;
        }

        protected override string[] Includes()
        {
            return new string[] {
                "AccountRoles.Role"
            };
        }

        protected override string[] SearchIncludes()
        {
            return new string[] {
                "AccountRoles.Role"
            };
        }

        protected override IQueryable<Account> SearchQueryInternal(IQueryable<Account> querable, AccountsQuery searchQuery)
        {
            querable = !string.IsNullOrEmpty(searchQuery.FirstName) ?
                querable.Where(e => e.FirstName.Contains(searchQuery.FirstName)) : querable;
            querable = !string.IsNullOrEmpty(searchQuery.LastName) ?
                querable.Where(e => e.LastName.Contains(searchQuery.LastName)) : querable;
            querable = !string.IsNullOrEmpty(searchQuery.FullName) ?
                querable.Where(e => e.FullName.Contains(searchQuery.FullName)) : querable;
            querable = !string.IsNullOrEmpty(searchQuery.Email) ?
                querable.Where(e => e.Email.Contains(searchQuery.Email)) : querable;
            querable = !string.IsNullOrEmpty(searchQuery.PhoneNumber) ?
                querable.Where(e => e.PhoneNumber.Contains(searchQuery.PhoneNumber)) : querable;
            querable = searchQuery.IsVerified != null ?
                querable.Where(e => e.IsVerified == searchQuery.IsVerified) : querable;

            // querable = querable.Where(e => e.IsExternal == searchQuery.IsExternal);

            // remove SeperAdmins from result set
            querable = querable.Where(e => !e.IsSuperAdmin);

            // defult ortder
            querable = querable.OrderByDescending(x => x.Id);

            return querable;
        }

        private async Task CheckIfDeleteAccountIsSuperAdmin(int id)
        {
            var Account = await Get(id);

            if (Account.IsSuperAdmin)
                throw new ValidationError(new List<ApiError>() { new ApiError(400, _stringLocalizer["CannotDeleteSuperAdminAccount"]) });
        }
    }
}
