using Common.Interface;
using Common.Model;
using HashidsNet;
using IdentityModel;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Common.Filters
{
    public class AuthenticationFilter : IActionFilter
    {
        private readonly IDatabaseContext ctx;
        private readonly IHashids _hashids;

        public AuthenticationFilter(IDatabaseContext ctx,
                                    IHashids hashids)
        {
            this.ctx = ctx;
            _hashids = hashids;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            ctx.CurrentAccount = new AuthenticatedAccount();

            var name = context.HttpContext.User.Identity.Name;

            if (!string.IsNullOrEmpty(name))
            {
                ctx.CurrentAccount.Email = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Email).Value; ;
                ctx.CurrentAccount.Id = _hashids.DecodeSingle(context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Id).Value);
                ctx.CurrentAccount.FirstName = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.GivenName).Value;
                ctx.CurrentAccount.LastName = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.FamilyName).Value;
                ctx.CurrentAccount.FullName = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Name).Value;

                ctx.CurrentAccount.Roles = context.HttpContext.User.Claims
                    .Where(c => c.Type == "role")
                    .Select(c => c.Value)
                    .ToList();

                ctx.Token = context.HttpContext.Request.Headers["Authorization"];
            }
        }
    }
}

