using Common.Model.Commands;
using tenant = Core.Tenants.DAL.Entity.Tenant;

namespace Core.Tenants.Service.AccountTenantMembership.Command
{
    public class CreateAccountTenantMembershipCommand
    {
        public string AccountId { get; set; }
        public string TenantId { get; set; }
        public bool IsDefault { get; set; } = false;
    }
}
