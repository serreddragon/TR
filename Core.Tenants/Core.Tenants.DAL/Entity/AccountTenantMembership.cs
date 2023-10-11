using Common.Model.Enitites;

namespace Core.Tenants.DAL.Entity
{
    public class AccountTenantMembership : BaseEntity
    {
        public int AccountId { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public bool IsDefault { get; set; } = false;
    }
}
