using Common.Infrastructure.HttpClient.Extension;
using Common.Model.Response;
using Core.Tenants.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Core.Tenants.Infrastructure.HttpClients
{
    public class AccountsApi : IAccountsApi
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public AccountsApi(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        public async Task<Response<bool>> AssignInitialRoles(AssignAccountTenantRolesCommand request)
        {
            Response<bool> result = new Response<bool>();

            var assignRolesJson = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var assignRolesUrl = _configuration["AccountApiConfiguration:AssignInitialRolesActionPath"].Trim().TrimEnd('/').TrimStart('/');

            _client.SetM2MBearerToken();
            HttpResponseMessage response = await _client.PostAsync($"/{assignRolesUrl}", assignRolesJson);

            if (response.IsSuccessStatusCode)
            {
                var stringResult = await response.Content.ReadAsStringAsync();

                result = JsonConvert.DeserializeObject<Response<bool>>(stringResult);
            }

            return result;
        }

        public async Task<Response<bool>> CreateTenantRoles(CreateRoleCommand request)
        {
            Response<bool> result = new Response<bool>();

            var createRolesJson = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var createRolesUrl = _configuration["AccountApiConfiguration:CreateRolesActionPath"].Trim().TrimEnd('/').TrimStart('/');
            _client.SetM2MBearerToken();
            HttpResponseMessage response = await _client.PostAsync($"/{createRolesUrl}", createRolesJson);

            if (response.IsSuccessStatusCode)
            {
                var stringResult = await response.Content.ReadAsStringAsync();

                result = JsonConvert.DeserializeObject<Response<bool>>(stringResult);
            }

            return result;
        }

    }
}
