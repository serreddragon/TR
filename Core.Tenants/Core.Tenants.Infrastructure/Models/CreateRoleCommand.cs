
namespace Core.Tenants.Infrastructure.Models
{
    public class CreateRoleCommand
    {
        public string TenantId { get; set; }
        public string RoleName { get; set; }
    }
}
