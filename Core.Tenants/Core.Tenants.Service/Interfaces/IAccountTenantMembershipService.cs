using Common.Interfaces;
using Core.Tenants.DAL;
using Core.Tenants.Service.AccountTenantMembership.Command;
using Core.Tenants.Service.AccountTenantMembership.Querry.Request;
using Core.Tenants.Service.AccountTenantMembership.Querry.Response;
using accountTenantMembership = Core.Tenants.DAL.Entity.AccountTenantMembership;

namespace Core.Tenants.Service.Interfaces
{
    public interface IAccountTenantMembershipService : IBaseService<TenantsDbContext, accountTenantMembership, AccountTenantMembershipResponse, AccountTenantMembershipBaseResponse, AccountTenantMembershipQuery>
    {

        public Task<List<AccountTenantMembershipResponse>> GetByTenantId(string tenantId);
        public Task<List<AccountTenantMembershipResponse>> GetByAccountId(string accountId);
        public Task<accountTenantMembership> Create(CreateAccountTenantMembershipCommand cmd, bool existingTenant);
        public Task<accountTenantMembership> Update(UpdateAccountTenantMembershipCommand cmd);
    }
}
