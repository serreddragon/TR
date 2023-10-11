using Common.Constants;
using Common.Model.Commands;
using Core.Accounts.DAL;
using FluentValidation;
using HashidsNet;
using Localization.Resources;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;

namespace Core.Accounts.Service
{
    public class UpdateAccountCommand
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
    }

    public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
    {
        private readonly AccountsDbContext _ctx;
        private readonly IHashids _hashids;
        public UpdateAccountCommandValidator(AccountsDbContext ctx, IHashids hashids,
                                          IStringLocalizer<SharedResource> stringLocalizer)
        {
            _ctx = ctx;
            _hashids = hashids;

            RuleFor(x => x.Id).Equal(_hashids.Encode(_ctx.CurrentAccount.Id));
            RuleFor(cmd => cmd.FirstName).NotEmpty();
            RuleFor(cmd => cmd.LastName).NotEmpty();
            RuleFor(cmd => cmd.PhoneNumber).NotEmpty();
        }
    }
}
