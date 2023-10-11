using Common.Interfaces;
using Core.Accounts.DAL;
using Core.Accounts.DAL.Entity;
using Core.Accounts.Service.Roles.Command;
using Core.Accounts.Service.Roles.Query.Request;
using Core.Accounts.Service.Roles.Query.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Accounts.Service.Interface
{
    public interface IRolesService : IBaseService<AccountsDbContext, Role, RoleResponse, RoleBaseResponse, RoleQuery>
    {
        Task<bool> CreateRoles(CreateRoleCommand cmd);
        Task<bool> CreateRole(CreateRoleCommand command);
        Task<List<RoleResponse>> GetAll();
        Task<List<RoleResponse>> GetRolesByTenantId(string tenantId);
    }
}
