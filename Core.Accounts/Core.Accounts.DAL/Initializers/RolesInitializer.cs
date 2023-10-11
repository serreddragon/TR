using Common.Constants;
using Core.Accounts.DAL.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Core.Accounts.DAL.Initializers
{
    public class RolesInitializer
    {
        private readonly AccountsDbContext _ctx;

        public RolesInitializer(AccountsDbContext ctx)
        {
            _ctx = ctx;
        }

        public void Initialize()
        {
            var existingRoles = _ctx.Roles.ToList();

            foreach (var role in InitializeRoles)
            {
                var existing = existingRoles.FirstOrDefault(r => r.Name == role.Name);

                if (existing == null)
                {
                    _ctx.Roles.Add(
                        new Role
                        {
                            Name = role.Name,
                            Description = role.Description,
                        });
                }
                else
                {
                    existing.Name = role.Name;
                    existing.Description = role.Description;
                }
            }

            _ctx.SaveChanges();
        }

        public void AddAllRolesToAccount(Account account)
        {
            var roles = _ctx.Roles.ToList();

            var existingAccountRoles = _ctx.AccountRoles.Where(ur => ur.AccountId == account.Id)?.ToList();

            foreach (var role in roles)
            {
                if (!existingAccountRoles.Any(ur => ur.RoleId == role.Id))
                {
                    _ctx.AccountRoles.Add(new AccountRole { AccountId = account.Id, RoleId = role.Id });
                }
            }
        }

        public static List<Role> InitializeRoles => new()
        {
            new Role
            {
                Name = RoleConstants.SuperAdminRole,
                Description = "Super Administrator"
            },
            new Role
            {
                Name = RoleConstants.AdminRole,
                Description = "Administrator"
            },
            new Role
            {
                Name = RoleConstants.AccountRole,
                Description = "Account"
            },
            new Role
            {
                Name = RoleConstants.ClientRole,
                Description = "Client"
            }
        };
    }
}
