using Common.Model.Search;

namespace Core.Accounts.Service.Roles.Query.Request
{
    public class RoleQuery : BaseSearchQuery
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
