using Core.Accounts.DAL;
using FluentValidation;
using Localization.Resources;
using Microsoft.Extensions.Localization;

namespace Core.Accounts.Service
{
    public class AssignAccountTenantRolesCommand
    {
        public string AccountId { get; set; }

        public string TenantId { get; set; }

        public bool IsExistingTenant { get; set; }
    }

    public class AssignAccountTenantRolesCommandValidator : AbstractValidator<AssignAccountTenantRolesCommand>
    {
        private readonly AccountsDbContext _ctx;
        public AssignAccountTenantRolesCommandValidator(AccountsDbContext ctx,
                                          IStringLocalizer<SharedResource> stringLocalizer)
        {
            _ctx = ctx;

            RuleFor(cmd => cmd.AccountId).NotEmpty();
            RuleFor(cmd => cmd.TenantId).NotEmpty();
        }
    }
}
