using Common.Utilities;
using Core.Accounts.DAL;
using HashidsNet;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Core.IdentityServer.Validation
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly AccountsDbContext _ctx;
        private readonly IHashids _hashids;

        public ResourceOwnerPasswordValidator(AccountsDbContext ctx,
                                              IHashids hashids)
        {
            _ctx = ctx;
            _hashids = hashids;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var Account = await _ctx.Accounts.FirstOrDefaultAsync(x => x.Email == context.UserName);

            if (Account != null && Account.IsVerified && SecurePasswordHasher.Verify(context.Password, Account.Password))
            {
                context.Result = new GrantValidationResult(
                    subject: _hashids.Encode(Account.Id),
                    authenticationMethod: "CustomResourceOwnerPassword",
                    claims: new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Email, Account.Email),
                    });
                return;
            }

            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
        }
    }
}
