using Core.Tenants.DAL;

namespace Core.Tenants.Service.Common
{
    public class TenantValidations
    {
        private readonly TenantsDbContext _ctx;
        public TenantValidations(TenantsDbContext context)
        {
            _ctx = context;
        }
        public bool TenantNameExists(string name)
        {
            return _ctx.Tenants.Any(t => t.Name.ToLower() == name.ToLower());
        }
        
    }
}
