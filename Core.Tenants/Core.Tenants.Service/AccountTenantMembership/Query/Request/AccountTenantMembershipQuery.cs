using Common.Model.Search;

namespace Core.Tenants.Service.AccountTenantMembership.Querry.Request
{
    public class AccountTenantMembershipQuery : BaseSearchQuery
    {
        public int AccountId { get; set; }
        public int TenantId { get; set; }
    }
}
