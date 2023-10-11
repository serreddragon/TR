using Core.Tenants.DAL;
using FluentValidation;
using HashidsNet;
using Localization.Resources;
using Microsoft.Extensions.Localization;

namespace Core.Tenants.Service.Tenant.Command
{
    public class UpdateTenantCommandValidator : AbstractValidator<UpdateTenantCommand>
    {
        private readonly TenantsDbContext _ctx;
        IStringLocalizer<SharedResource> _stringLocalizer;
        private readonly IHashids _hashids;

        public UpdateTenantCommandValidator(TenantsDbContext context, IStringLocalizer<SharedResource> stringLocalizer)
        {
            _ctx = context;
            _stringLocalizer = stringLocalizer;

            RuleFor(cmd => cmd.Name).NotEmpty();
            RuleFor(cmd => cmd).Must(cmd => TenantExists(cmd));
            RuleFor(cmd => cmd).Must(cmd => !TenantNameExists(cmd));
        }

        private bool TenantExists(UpdateTenantCommand command)
        {
            return _ctx.Tenants.Any(t => t.Id == _hashids.DecodeSingle(command.Id));
        }

        private bool TenantNameExists(UpdateTenantCommand command)
        {

            return _ctx.Tenants.Any(t => t.Name.ToLower() == command.Name.ToLower()); ;
        }
    }
}
