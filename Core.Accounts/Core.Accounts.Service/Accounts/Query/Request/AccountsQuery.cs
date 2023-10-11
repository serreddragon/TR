using Common.Model.Search;

namespace Core.Accounts.Service.Accounts.Query.Request
{
    public class AccountsQuery : BaseSearchQuery
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool? IsVerified { get; set; }

        public bool IsExternal { get; set; }
    }
}
