using Common.Model.Response;
using Core.Accounts.DAL.Constants;

namespace Core.Accounts.Service.Accounts.Query.Response
{
    public class AccountsBaseResponse : BaseResponse
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public AccountVerificationStatus Status { get; set; }

        public string StatusDisplay { get; set; }

        public bool IsVerified { get; set; }

    }
}
