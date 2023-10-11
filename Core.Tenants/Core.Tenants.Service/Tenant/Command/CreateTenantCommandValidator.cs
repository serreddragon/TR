using Core.Tenants.DAL;
using FluentValidation;
using Localization.Resources;
using Microsoft.Extensions.Localization;

namespace Core.Tenants.Service.Tenant.Command
{
    public class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
    {
        private readonly TenantsDbContext _ctx;
        IStringLocalizer<SharedResource> _stringLocalizer;

        public CreateTenantCommandValidator(TenantsDbContext context, IStringLocalizer<SharedResource> stringLocalizer)
        {
            _ctx = context;
            _stringLocalizer = stringLocalizer;

            RuleFor(cmd => cmd.Name).NotEmpty();
            RuleFor(cmd => cmd).Must(cmd => !TenantExists(cmd));
        }

        private bool TenantExists(CreateTenantCommand command)
        {
            return _ctx.Tenants.Any(t => t.Name.ToLower() == command.Name.ToLower());
        }
    }
}
