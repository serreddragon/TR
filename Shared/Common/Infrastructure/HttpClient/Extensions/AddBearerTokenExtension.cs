using IdentityModel.Client;
using System.Threading.Tasks;
using http = System.Net.Http;

namespace Common.Infrastructure.HttpClient.Extension
{
    public static class AddBearerTokenExtension
    {
        private const string ISAdress = "https://localhost:5001";
        private const string ClientId = "M2M_client";
        private const string secret = "secret";

        public static void SetM2MBearerToken(this http.HttpClient client)
        {
            var identityClient = new http.HttpClient();
            var disco = Task.Run(() => identityClient.GetDiscoveryDocumentAsync(ISAdress)).Result;
           
            if (disco.IsError)
            {
                return;
            }

            var tokenResponse = Task.Run(() => identityClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = ClientId,
                ClientSecret = secret
            })).Result;

            if (!tokenResponse.IsError)
                client.SetBearerToken(tokenResponse.AccessToken);
        }
    }
}
