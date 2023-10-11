using Localization.Resources;
using System.ComponentModel.DataAnnotations;

namespace Core.Accounts.DAL.Constants
{
    public enum AccountVerificationStatus
    {
        [Display(Name = nameof(SharedResource.AccountVeificationStatus_EmailNotVerified), ResourceType = typeof(SharedResource))]
        NotVerified = 1,
        [Display(Name = nameof(SharedResource.AccountVeificationStatus_PasswordResetRequested), ResourceType = typeof(SharedResource))]
        PasswordResetRequested = 2,

        // add more statuses here
        [Display(Name = nameof(SharedResource.AccountVeificationStatus_Verified), ResourceType = typeof(SharedResource))]
        Verified = 100
    }
}
