using Common.Model.Response;
using Core.Accounts.Infrastructure.Models;


namespace Core.Accounts.Infrastructure.HttpClients
{

    public interface ITenantsApi
    {
        Task<Response<string>> SignUpTenant(string accountId, CreateTenantCommand request);
    }
}
