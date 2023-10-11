using Common.Infrastructure.HttpClient.Extension;
using Common.Model.Response;
using Core.Accounts.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Core.Accounts.Infrastructure.HttpClients
{
    public class TenantsApi : ITenantsApi
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public TenantsApi(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        public async Task<Response<string>> SignUpTenant(string accountId, CreateTenantCommand request)
        {
            var result = string.Empty;

            var requestJson = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var actionUrl = _configuration["TenantApiConfiguration:SignUpTenantActionPath"].Trim().TrimEnd('/').TrimStart('/');

            _client.SetM2MBearerToken();

            HttpResponseMessage response = await _client.PostAsync($"/{actionUrl}/?accountId={accountId}", requestJson);

            if (response.IsSuccessStatusCode)
            {
                var stringResult = await response.Content.ReadAsStringAsync();

                result = JsonConvert.DeserializeObject<Response<TenantResponse>>(stringResult).Data.Id;
            }
            return new Response<string>(result);
        }

    }
}
