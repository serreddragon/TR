using Core.Accounts.DAL;
using Core.Accounts.Service.Common;
using FluentValidation;
using Localization.Resources;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Accounts.Service
{
    public class RegisterAccountCommand
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }
    }

    public class RegisterAccountCommandValidator : AbstractValidator<RegisterAccountCommand>
    {
        private readonly AccountsDbContext _ctx;
        private readonly AuthValidations _authValidations;

        public RegisterAccountCommandValidator(AccountsDbContext ctx,
                                            AuthValidations authValidations,
                                            IStringLocalizer<SharedResource> stringLocalizer)
        {
            _ctx = ctx;
            _authValidations = authValidations;

            RuleFor(cmd => cmd.FirstName).NotEmpty();
            RuleFor(cmd => cmd.LastName).NotEmpty();

            RuleFor(cmd => cmd.PhoneNumber).NotEmpty();
            RuleFor(cmd => cmd.Email).NotEmpty().EmailAddress();

            // Password can be empty for Invite Flow
            // RuleFor(cmd => cmd.Password).NotEmpty();

            RuleFor(cmd => cmd.Password)
                .Must(password => AuthValidations.IsPasswordOk(password))
                .When(cmd => !string.IsNullOrEmpty(cmd.Password))
                .WithMessage(stringLocalizer["PasswordRules"]);

            RuleFor(cmd => cmd)
               .MustAsync((cmd, cancellationToken) => _authValidations.AccountWithPhoneNumberNotExistsAsync(cmd.PhoneNumber))
               .WithMessage(stringLocalizer["InvalidRegisterAttempt_PhoneNumberAlreadyUsed"]);

            RuleFor(cmd => cmd)
                .MustAsync((cmd, cancellationToken) => _authValidations.AccountWithEmailNotExistsAsync(cmd.Email))
                .WithMessage(stringLocalizer["InvalidRegisterAttempt_EmailAlreadyUsed"]);

            RuleFor(cmd => cmd)
                .MustAsync((cmd, cancellationToken) => CanRegister(cmd))
                .WithMessage(stringLocalizer["InvalidRegisterAttempt_UnknownError"]);

        }

        private async Task<bool> CanRegister(RegisterAccountCommand cmd)
        {
            return await _authValidations.AccountWithEmailNotExistsAsync(cmd.Email) && await _authValidations.AccountWithPhoneNumberNotExistsAsync(cmd.PhoneNumber);
        }
    }
}
