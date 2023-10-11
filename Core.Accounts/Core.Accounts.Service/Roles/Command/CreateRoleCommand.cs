using Common.Model.Commands;

namespace Core.Accounts.Service.Roles.Command
{
    public class CreateRoleCommand
    {
        public string TenantId { get; set; }
        public string RoleName { get; set; }
    }
}
