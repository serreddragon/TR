using Core.Accounts.DAL;
using Core.Accounts.Service.Roles.Command;
using FluentValidation;
using Localization.Resources;
using Microsoft.Extensions.Localization;

namespace Core.Tenants.Service.Tenant.Command
{
    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        IStringLocalizer<SharedResource> _stringLocalizer;
        AccountsDbContext _ctx;

        public CreateRoleCommandValidator(AccountsDbContext context, IStringLocalizer<SharedResource> stringLocalizer)
        {
            _ctx = context;
            _stringLocalizer = stringLocalizer;

            RuleFor(cmd => cmd.TenantId).NotEmpty();
        }
    }
}
