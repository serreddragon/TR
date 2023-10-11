using AutoMapper;
using Common.Services;
using Core.Tenants.DAL;
using Core.Tenants.Infrastructure.HttpClients;
using Core.Tenants.Infrastructure.Models;
using Core.Tenants.Service.AccountTenantMembership.Command;
using Core.Tenants.Service.Interfaces;
using Core.Tenants.Service.Tenant.Command;
using Core.Tenants.Service.Tenant.Querry.Request;
using Core.Tenants.Service.Tenant.Querry.Response;
using HashidsNet;
using Localization.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Transactions;
using TenantType = Core.Tenants.DAL.Entity.Tenant;

namespace Core.Tenants.Service.Tenant
{
    public class TenantService : BaseService<TenantsDbContext, TenantType, TenantResponse, TenantBaseResponse, TenantQuery>, ITenantService
    {
        private readonly CreateTenantCommandValidator _createTenantCommandValidator;
        private readonly IAccountsApi _accountsApi;
        private readonly IAccountTenantMembershipService _accountTenantMembershipService;

        public TenantService(IAccountTenantMembershipService accountTenantMembershipService, CreateTenantCommandValidator createTenantCommandValidator, IAccountsApi accountsApi, TenantsDbContext tenantDBContext, IMapper mapper, IStringLocalizer<SharedResource> stringLocalizer, IHashids hashids) : base(tenantDBContext, mapper, stringLocalizer, hashids)
        {
            _accountTenantMembershipService = accountTenantMembershipService;
            _createTenantCommandValidator = createTenantCommandValidator;
            _accountsApi = accountsApi;
        }

        public async Task<TenantResponse> GetByName(string tenantName)
        {
            var existingTenant = await _ctx.Tenants.FirstOrDefaultAsync(x => x.Name.ToLower() == tenantName.ToLower());

            return existingTenant == default ? new TenantResponse() : _mapper.Map<TenantResponse>(existingTenant);
        }

        public async Task<List<TenantResponse>> GetAllTenants()
        {
            var tenants = await _ctx.Tenants.ToListAsync();
            return _mapper.Map<List<TenantResponse>>(tenants);
        }

        public async Task<TenantResponse> CreateTenant(CreateTenantCommand insertCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                await _createTenantCommandValidator.ValidateAsync(insertCommand);

                var tenant = new TenantType { Name = insertCommand.Name, Description = insertCommand.Description };
                var inserted = await _ctx.Tenants.AddAsync(tenant);
                await _ctx.SaveChangesAsync();

                CreateRoleCommand createRole = new CreateRoleCommand { TenantId = _hashids.Encode(inserted.Entity.Id) };
                var result = await _accountsApi.CreateTenantRoles(createRole);

                if (result.Data)
                    scope.Complete();

                return _mapper.Map<TenantResponse>(inserted?.Entity);
            }
        }

        public async Task<bool> UpdateTenant(UpdateTenantCommand updateCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                var tenant = _ctx.Tenants.First(x => x.Id == _hashids.DecodeSingle(updateCommand.Id));
                tenant.Name = updateCommand.Name;
                tenant.UpdatedById = _ctx.CurrentAccount.Id;
                var updated = await _ctx.SaveChangesAsync();
                scope.Complete();
                return _mapper.Map<bool>(updated);
            }
        }

        protected override IQueryable<TenantType> SearchQueryInternal(IQueryable<TenantType> querable, TenantQuery searchQuery)
        {
            querable = !string.IsNullOrEmpty(searchQuery.Name) ?
                  querable.Where(e => e.Name.ToLower().Contains(searchQuery.Name.ToLower())) : querable;

            return querable;
        }

        public async Task<TenantResponse> SignUpTenant(string accountId, CreateTenantCommand createTenantCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                if (string.IsNullOrEmpty(accountId))
                    throw new ArgumentNullException(nameof(accountId));

                _createTenantCommandValidator.Validate(createTenantCommand);

                var tenant = await CreateTenant(createTenantCommand);

                var accountTenantMembershipCommand = new CreateAccountTenantMembershipCommand { AccountId = accountId, TenantId = tenant.Id, IsDefault = true };

                var membership = await _accountTenantMembershipService.Create(accountTenantMembershipCommand, false);

                if (membership != default)
                    scope.Complete();

                return tenant;
            }
        }
    }
}
