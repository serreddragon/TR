using AutoMapper;
using Common.Services;
using Core.Tenants.DAL;
using Core.Tenants.Infrastructure.HttpClients;
using Core.Tenants.Infrastructure.Models;
using Core.Tenants.Service.AccountTenantMembership.Command;
using Core.Tenants.Service.AccountTenantMembership.Querry.Request;
using Core.Tenants.Service.AccountTenantMembership.Querry.Response;
using Core.Tenants.Service.Interfaces;
using HashidsNet;
using Localization.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Transactions;
using accountTenantMembership = Core.Tenants.DAL.Entity.AccountTenantMembership;

namespace Core.Tenants.Service.AccountTenantMembership
{
    public class AccountTenantMembershipService : BaseService<TenantsDbContext, accountTenantMembership, AccountTenantMembershipResponse, AccountTenantMembershipBaseResponse, AccountTenantMembershipQuery>, IAccountTenantMembershipService
    {
        private readonly CreateAccountTenantMembershipCommandValidator _insertAccountTenantMembershipValidator;
        private readonly UpdateAccountTenantMembershipCommandValidator _updateAccountTenantMembershipValidator;
        private readonly IAccountsApi _accountsApi;

        public AccountTenantMembershipService(TenantsDbContext ctx, IMapper mapper, IAccountsApi accountsApi, IStringLocalizer<SharedResource> stringLocalizer, IHashids hashIds, CreateAccountTenantMembershipCommandValidator insertAccountTenantMembershipValidator, UpdateAccountTenantMembershipCommandValidator updateAccountTenantMembershipValidator) : base(ctx, mapper, stringLocalizer, hashIds)
        {
            _insertAccountTenantMembershipValidator = insertAccountTenantMembershipValidator;
            _updateAccountTenantMembershipValidator = updateAccountTenantMembershipValidator;
            _accountsApi = accountsApi;
        }

        public async Task<accountTenantMembership> Create(CreateAccountTenantMembershipCommand cmd, bool existingTenant = true)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                await _insertAccountTenantMembershipValidator.ValidateAsync(cmd);

                var accountTenantMembership = await _ctx.AccountTenantMemberships.FirstOrDefaultAsync(x => x.AccountId == _hashids.DecodeSingle(cmd.AccountId) && x.TenantId == _hashids.DecodeSingle(cmd.TenantId));

                if (accountTenantMembership == default)
                {
                    accountTenantMembership = _mapper.Map<accountTenantMembership>(cmd);
                    await _ctx.AddAsync(accountTenantMembership);
                }

                var assignInitialRolesCommand = _mapper.Map<AssignAccountTenantRolesCommand>(cmd);
                assignInitialRolesCommand.IsExistingTenant = existingTenant;

                var result = await _accountsApi.AssignInitialRoles(assignInitialRolesCommand);

                if (result.Data)
                {
                    await _ctx.SaveChangesAsync();
                    scope.Complete();
                }
                return accountTenantMembership;
            }
        }

        public async Task<List<AccountTenantMembershipResponse>> GetByAccountId(string accountId)
        {
            var membership = GetQueryable(Includes()).Where(s => s.AccountId == _hashids.DecodeSingle(accountId)).ToList();
            return _mapper.Map<List<AccountTenantMembershipResponse>>(membership);
        }

        public async Task<List<AccountTenantMembershipResponse>> GetByTenantId(string tenantId)
        {
            var membership = GetQueryable(Includes()).Where(s => s.TenantId == _hashids.DecodeSingle(tenantId)).ToList();
            return _mapper.Map<List<AccountTenantMembershipResponse>>(membership);
        }

        public async Task<accountTenantMembership> Update(UpdateAccountTenantMembershipCommand cmd)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                await _updateAccountTenantMembershipValidator.ValidateAsync(cmd);

                var existingMembership = await GetQueryable().FirstOrDefaultAsync(x => x.Id == _hashids.DecodeSingle(cmd.Id));

                if (existingMembership != null)
                {
                    existingMembership.UpdatedById = _ctx.CurrentAccount.Id;
                    existingMembership.AccountId = _hashids.DecodeSingle(cmd.AccountId);
                    existingMembership.TenantId = _hashids.DecodeSingle(cmd.TenantId);
                    existingMembership.IsDefault = cmd.IsDefault;
                    await _ctx.SaveChangesAsync();
                }
                scope.Complete();
                return existingMembership;
            }
        }

        protected override IQueryable<accountTenantMembership> SearchQueryInternal(IQueryable<accountTenantMembership> querable, AccountTenantMembershipQuery searchQuery)
        {
            querable = searchQuery.AccountId != 0 ?
               querable.Where(e => e.AccountId == searchQuery.AccountId) : querable;
            querable = searchQuery.TenantId != 0 ?
             querable.Where(e => e.TenantId == searchQuery.TenantId) : querable;
            return querable;
        }

        protected override string[] Includes()
        {
            return new string[] { "Tenant" };
        }
    }
}
