using Common.Model.Response;

namespace Core.Tenants.Service.AccountTenantMembership.Querry.Response
{
    public class AccountTenantMembershipBaseResponse : BaseResponse
    {
        public string AccountId { get; set; }
        public string TenantId { get; set; }
        public bool IsDefault { get; set; }
    }
}
