using Common.Model.Commands;
using Core.Accounts.DAL;
using Core.Accounts.DAL.Entity;
using FluentValidation;
using Localization.Resources;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;

namespace Core.Accounts.Service
{
    public class CreateAccountRolesCommand
    {
        public string AccountId { get; set; }

        public List<string> RoleIds { get; set; }
    }

    public class CreateAccountRolesCommandValidator : AbstractValidator<CreateAccountRolesCommand>
    {
        private readonly AccountsDbContext _ctx;
        public CreateAccountRolesCommandValidator(AccountsDbContext ctx,
                                          IStringLocalizer<SharedResource> stringLocalizer)
        {
            _ctx = ctx;

            RuleFor(cmd => cmd.RoleIds).NotEmpty();
        }
    }
}
