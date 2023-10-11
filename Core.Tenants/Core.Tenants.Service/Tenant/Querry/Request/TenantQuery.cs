using Common.Model.Search;

namespace Core.Tenants.Service.Tenant.Querry.Request
{
    public class TenantQuery : BaseSearchQuery
    {
        public string Id { get; set; }
        public string Name { get; set; }
     }
}
