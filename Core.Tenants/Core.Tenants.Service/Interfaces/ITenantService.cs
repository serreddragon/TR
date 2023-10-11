using Common.Interfaces;
using Core.Tenants.DAL;
using Core.Tenants.Service.Tenant.Command;
using Core.Tenants.Service.Tenant.Querry.Request;
using Core.Tenants.Service.Tenant.Querry.Response;
using TenantType = Core.Tenants.DAL.Entity.Tenant;

namespace Core.Tenants.Service.Interfaces
{
    public interface ITenantService : IBaseService<TenantsDbContext, TenantType, TenantResponse, TenantBaseResponse, TenantQuery>
    {
        public Task<List<TenantResponse>> GetAllTenants();
        public Task<TenantResponse> SignUpTenant(string accountId, CreateTenantCommand createTenantCommand);
        public Task<TenantResponse> CreateTenant(CreateTenantCommand insertCommand);
        public Task<bool> UpdateTenant(UpdateTenantCommand updateCommand);
        public Task<TenantResponse> GetByName(string tenantName);       
    }
}
