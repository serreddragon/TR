namespace Core.Accounts.DAL.Constants
{
    public class AccountConstants
    {
        public const string ApiName = "core.accounts.api";

        public const int PasswordLength = 6;

        public const int VerificationTokenValidityDuration = 24;

        public const string VerificationTokenPrefix = "verify";

        public const int ResetTokenValidityDuration = 24;

        public const string ResetTokenPrefix = "reset";
    }
}
