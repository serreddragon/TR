namespace Core.Tenants.Infrastructure.Models
{
    public class AssignAccountTenantRolesCommand
    {
        public string AccountId { get; set; }
        public string TenantId { get; set; }
        public bool IsExistingTenant { get; set; }
    }
}
