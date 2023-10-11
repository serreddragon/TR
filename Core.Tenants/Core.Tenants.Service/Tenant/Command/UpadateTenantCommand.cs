using Common.Model.Commands;

namespace Core.Tenants.Service.Tenant.Command
{
    public class UpdateTenantCommand
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
