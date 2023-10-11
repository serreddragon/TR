using Core.Accounts.DAL;
using Core.Accounts.Service.Common;
using FluentValidation;
using Localization.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace Core.Accounts.Service.Accounts.Command
{
    public class ChangePasswordCommand
    {
        public string Email { get; set; }

        public string Token { get; set; }

        public string Password { get; set; }

    }

    public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
    {
        private readonly AccountsDbContext _ctx;
        private readonly AuthValidations _authValidations;
        public ChangePasswordValidator(AccountsDbContext ctx,
                                       AuthValidations authValidations,
                                       IStringLocalizer<SharedResource> stringLocalizer)
        {
            _ctx = ctx;
            _authValidations = authValidations;

            RuleFor(cmd => cmd.Email).NotEmpty().EmailAddress();
            RuleFor(cmd => cmd.Token).NotEmpty();
            RuleFor(cmd => cmd.Password).NotEmpty();

            RuleFor(cmd => cmd.Password)
                .Must(password => AuthValidations.IsPasswordOk(password))
                .When(cmd => !string.IsNullOrEmpty(cmd.Password))
                .WithMessage(stringLocalizer["PasswordRules"]);

            RuleFor(cmd => cmd)
                .MustAsync((cmd, cancellationToken) => Verify(cmd))
                .WhenAsync((cmd, cancellationToken) => _authValidations.AccountWithEmailExistsAsync(cmd.Email))
                .WithMessage(stringLocalizer["InvalidEmailVerificationAttempt_TokenExpired"]);
        }

        private async Task<bool> Verify(ChangePasswordCommand cmd)
        {
            var user = await _ctx.Accounts.FirstAsync(x => x.Email == cmd.Email);

            return user.ResetToken == cmd.Token && user.ResetExp >= System.DateTime.Now;
        }
    }
}
