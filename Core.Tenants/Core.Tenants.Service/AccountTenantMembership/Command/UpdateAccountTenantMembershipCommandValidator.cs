using Core.Tenants.DAL;
using FluentValidation;

namespace Core.Tenants.Service.AccountTenantMembership.Command
{
    public class UpdateAccountTenantMembershipCommandValidator : AbstractValidator<UpdateAccountTenantMembershipCommand>
    {
        private readonly TenantsDbContext _tenantsDbContext;
        public UpdateAccountTenantMembershipCommandValidator(TenantsDbContext dbContext)
        {
            _tenantsDbContext = dbContext;

            RuleFor(x => x.AccountId).NotEmpty();
            RuleFor(x => x.TenantId).NotEmpty();
        }
    }
}
