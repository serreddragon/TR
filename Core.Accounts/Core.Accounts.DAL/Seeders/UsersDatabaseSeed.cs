using Common.Constants;
using Common.Utilities;
using Core.Accounts.DAL.Entity;
using System.Linq;

namespace Core.Accounts.DAL.Seeders
{
    /// <summary>
    /// Populets db with some data for testing
    /// </summary>
    public class AccountsDatabaseSeed
    {
        private readonly AccountsDbContext _ctx;

        public AccountsDatabaseSeed(AccountsDbContext ctx)
        {
            _ctx = ctx;
        }

        public void Seed()
        {
            InsertAccounts();
        }

        private void InsertAccounts()
        {
            var customerEmail = "test@test.test";
            //var customerRole = _ctx.Roles.First(r => r.Name == RoleConstants.ClientRole);
            var customerAccount = _ctx.Accounts.FirstOrDefault(u => u.Email == customerEmail);

            if (customerAccount == null)
            {
                _ctx.Accounts.Add(new Account
                {
                    FirstName = "TestName",
                    LastName = "TeestSurname",
                    PhoneNumber = "00987654321",
                    Email = customerEmail,
                    Status = Constants.AccountVerificationStatus.Verified,
                    Password = SecurePasswordHasher.Hash("Pass123!"),
                    //AccountRoles = new List<AccountRole>() { new AccountRole { Role = customerRole } }
                });

                _ctx.SaveChanges();
            }
        }
    }
}
