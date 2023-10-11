using Common.Utilities;
using Core.Accounts.DAL.Constants;
using Core.Accounts.DAL.Entity;
using System.Linq;

namespace Core.Accounts.DAL.Initializers
{
    /// <summary>
    /// Populates db with data that must exist for app to work properly
    /// </summary>
    public class AccountsDatabaseInitializer
    {
        private readonly AccountsDbContext _ctx;
        private readonly RolesInitializer _rolesInitializer;

        public AccountsDatabaseInitializer(AccountsDbContext ctx,
                                        RolesInitializer rolesInitializer)
        {
            _ctx = ctx;
            _rolesInitializer = rolesInitializer;
        }

        public void Initialize()
        {
        //    InsertRoles();
        //    InsertAccounts();
        }

        private void InsertRoles()
        {
            _rolesInitializer.Initialize();
        }

        private void InsertAccounts()
        {
            var superAdminEmail = "super.admin@super.admin";
            var superAdmin = _ctx.Accounts.FirstOrDefault(e => e.Email == superAdminEmail);

            if (superAdmin == null)
            {
                _ctx.Accounts.Add(new Account
                {
                    FirstName = "Super",
                    LastName = "Admin",
                    PhoneNumber = "00123456789",
                    Email = superAdminEmail,
                    Status = AccountVerificationStatus.Verified,
                    Password = SecurePasswordHasher.Hash("Pass123!"),
                    //Roles = new List<AccountRole>() { new AccountRole { Role = _ctx.Roles.First(r => r.Name == RoleConstants.SuperAdminRole) }}
                }); ;

                _ctx.SaveChanges();
            }
        }
    }
}

