using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public class RolesClaimsTransformation : IClaimsTransformation
    {
        private readonly IHttpContextAccessor _httpContextAccessor;


        public RolesClaimsTransformation(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(_httpContextAccessor));
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            // Clone current identity
            var clone = principal.Clone();
            var newIdentity = (ClaimsIdentity)clone.Identity;
            var tenantId = _httpContextAccessor.HttpContext.Request.Headers["tenantId"];

            // Support AD and local accounts
            var tenantClaims = clone.Claims.Where(c => c.Type == tenantId).ToList();


            var roles = newIdentity.FindAll(JwtClaimTypes.Role).Where(r => r.Value != "M2M_role").ToList();

            if (string.IsNullOrEmpty(tenantId) && roles.Any())
            {
                throw new MessageHeaderException("Missing tenantId");
            }
            foreach (var role in roles)
            {
                newIdentity.RemoveClaim(role);
            }
            if (tenantClaims.Count() > 0)
            {
                foreach (var tenantClaim in tenantClaims)
                {
                    newIdentity.AddClaim(new Claim(JwtClaimTypes.Role, tenantClaim.Value));
                }
            }
            return clone;
        }
    }
}
