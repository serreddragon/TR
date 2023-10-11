using Common.Model.Response;

namespace Core.Accounts.Service.Roles.Query.Response
{
    public class RoleBaseResponse : BaseResponse
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
