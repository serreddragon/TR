using Common.Model.Response;
using Core.Tenants.Infrastructure.Models;

namespace Core.Tenants.Infrastructure.HttpClients
{
    public interface IAccountsApi
    {
        Task<Response<bool>> CreateTenantRoles(CreateRoleCommand request);
        Task<Response<bool>> AssignInitialRoles(AssignAccountTenantRolesCommand request);
    }
}
