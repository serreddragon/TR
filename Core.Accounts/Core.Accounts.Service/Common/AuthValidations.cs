using Core.Accounts.DAL.Constants;
using Core.Accounts.DAL;
using Core.Accounts.Service.Accounts.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Accounts.Service.Common
{
    public class AuthValidations
    {
        private readonly AccountsDbContext _ctx;

        public AuthValidations(AccountsDbContext ctx)
        {
            _ctx = ctx;
        }

        public bool AccountWithEmailExists(string email)
        {
            return _ctx.Accounts.Any(e => e.Email == email);
        }

        public async Task<bool> AccountWithEmailExistsAsync(string email)
        {
            return await _ctx.Accounts.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> AccountWithEmailNotExistsAsync(string email)
        {
            return !await _ctx.Accounts.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> AccountWithPhoneNumberNotExistsAsync(string phoneNumber)
        {
            phoneNumber = phoneNumber;

            return !await _ctx.Accounts.AnyAsync(u => u.PhoneNumber == phoneNumber);
        }

        public static bool IsPasswordOk(string password)
        {
            return password.Length >= AccountConstants.PasswordLength
                   && password.Any(char.IsDigit)
                   && password.Any(char.IsUpper)
                   && (password.Any(char.IsSymbol) || password.Any(char.IsPunctuation));
        }
    }
}
