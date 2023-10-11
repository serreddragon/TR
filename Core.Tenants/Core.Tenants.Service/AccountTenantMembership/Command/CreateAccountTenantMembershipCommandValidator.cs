using Core.Tenants.DAL;
using FluentValidation;
using HashidsNet;

namespace Core.Tenants.Service.AccountTenantMembership.Command
{
    public class CreateAccountTenantMembershipCommandValidator : AbstractValidator<CreateAccountTenantMembershipCommand>
    {
        private readonly TenantsDbContext _ctx;
        private readonly IHashids _hashids;
        public CreateAccountTenantMembershipCommandValidator(TenantsDbContext ctx, IHashids hashids)
        {
            _ctx = ctx;
            _hashids = hashids;

            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.AccountId).NotEmpty();
            RuleFor(cmd => cmd).Must(cmd => !HasDefault(cmd));
        }

        private bool HasDefault(CreateAccountTenantMembershipCommand cmd)
        {
            if (cmd.IsDefault)
                return _ctx.AccountTenantMemberships.Any(x => x.AccountId == _hashids.DecodeSingle(cmd.AccountId) && x.IsDefault == true);
            return false;
        }
    }
}
