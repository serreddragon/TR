using Common.Model.Response;

namespace Core.Tenants.Service.Tenant.Querry.Response
{
    public class TenantBaseResponse : BaseResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
