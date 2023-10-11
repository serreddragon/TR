using Common.Model.Commands;

namespace Core.Tenants.Service.AccountTenantMembership.Command
{
    public class UpdateAccountTenantMembershipCommand
    {
        public string Id { get; set; }
        public string AccountId { get; set; }
        public string TenantId { get; set; }
        public bool IsDefault { get; set; }
    }
}
