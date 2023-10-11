using Common.Constants;
using Core.Accounts.DAL.Entity;
using Core.Accounts.Service.Roles.Command;
using HashidsNet;
using System.Collections.Generic;

namespace Core.Accounts.Service.Roles.Extensions
{
    public static class CreateRolesExtension
    {

        public static List<Role> InitialTenantRoles(this CreateRoleCommand command, IHashids hashids)
        {
            var tenantId = hashids.DecodeSingle(command.TenantId);

            return new List<Role>() { new Role { Name = RoleConstants.SuperAdminRole, TenatId = tenantId }, new Role { Name = RoleConstants.AdminRole, TenatId = tenantId }, new Role { Name = RoleConstants.ClientRole, TenatId = tenantId }, new Role { Name = RoleConstants.AccountRole, TenatId = tenantId } };
        }
    }
}
