using Core.Accounts.DAL;
using Core.Tenants.DAL;
using HashidsNet;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Core.IdentityServer.Services
{
    /// <summary>
    /// The ProfileService class is responsible for adding/deleting claims from the Account profile to the access token.
    /// Note: don't forget to add/delete every new claim to the AccountClaims property of the Api resource definition in the IdentityServer config (Config.cs),
    /// and to delete the ApiResources table content in the database in order to seed the data properly.
    /// </summary>
    public class ProfileService : IProfileService
    {
        private readonly AccountsDbContext _ctx;
        private readonly TenantsDbContext _tenants;
        private readonly IHashids _hashids;

        public ProfileService(AccountsDbContext ctx, TenantsDbContext tenants,
                              IHashids hashids)
        {
            _ctx = ctx;
            _tenants = tenants;
            _hashids = hashids;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var id = context.Subject.FindFirstValue("sub");
            var scope = context.ValidatedRequest.Raw.Get("scope");

            var Account = await _ctx.Accounts.Include("AccountRoles.Role").FirstAsync(x => x.Id == _hashids.DecodeSingle(id));
            var memberships = _tenants.AccountTenantMemberships.Include("Tenant").Where(m => m.AccountId == _hashids.DecodeSingle(id)).ToList();

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Id, id),
                new Claim(JwtClaimTypes.GivenName, Account.FirstName ?? string.Empty),
                new Claim(JwtClaimTypes.FamilyName, Account.LastName ?? string.Empty),
                new Claim(JwtClaimTypes.Name, Account.FullName),
                new Claim(JwtClaimTypes.Email, Account.Email),
                new Claim(JwtClaimTypes.Confirmation, Account.IsVerified.ToString()),
            };

            foreach (var role in Account.Roles)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, role.Name));
            }

            foreach (var membership in memberships)
            {
                var tenantRoles = Account.Roles.Where(a => a.TenatId == membership.TenantId);
                foreach (var role in tenantRoles)
                    claims.Add(new Claim(_hashids.Encode(membership.TenantId), role.Name));
            }

            var defaultTenant = memberships.FirstOrDefault(t => t.IsDefault == true).Tenant.Id;

            claims.Add(new Claim("defaultTenant", _hashids.Encode(defaultTenant)));

            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var id = context.Subject.FindFirstValue("sub");

            var Account = await _ctx.Accounts.FirstOrDefaultAsync(x => x.Id == _hashids.DecodeSingle(id));

            context.IsActive = Account?.IsVerified ?? false;
        }
    }
}

