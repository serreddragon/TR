
using Common.Constants;
using Core.Accounts.Service.Roles.Query.Response;
using System.Collections.Generic;
using System.Linq;

namespace Core.Accounts.Service.Accounts.Query.Response
{
    public class AccountsResponse : AccountsBaseResponse
    {
        public List<RoleResponse> Roles { get; set; }

        public bool IsSuperAdmin => Roles.Any(x => x.Name == RoleConstants.SuperAdminRole);

    }
}
