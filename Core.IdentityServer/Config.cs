using IdentityModel;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Core.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() => new IdentityResource[]
{
};

        public static IEnumerable<ApiResource> GetApiResources() => new List<ApiResource>
        {
            new ApiResource("core.accounts.api", "Core.Accounts.API" )
            {
                Scopes = new [] { "accounts-api", "roles-api"}
            },
            new ApiResource("core.tenants.api", "Core.Tenants.API" )
            {
                Scopes = new [] { "tenants-api", "accounttenantmembership-api" }
            }
        };

        public static IEnumerable<ApiScope> GetApiScopes() => new List<ApiScope>
        {
            new ApiScope("sub", "sub", new [] { JwtClaimTypes.Id, JwtClaimTypes.PreferredUserName, JwtClaimTypes.Email, JwtClaimTypes.Name, JwtClaimTypes.Role } ),
            new ApiScope("accounts-api", "Accounts Endpoints"),
            new ApiScope("tenants-api", "Tenants Endpoints"),
            new ApiScope("accounttenantmembership-api", "AccountTenantMembership Endpoints"),
            new ApiScope("roles-api", "AccountTenantMembership Endpoints"),
        };

        public static IEnumerable<Client> GetClients(IConfigurationSection clientConfigs) => new List<Client>
        {
            new Client
            {
                ClientId = clientConfigs.GetSection("NonInteractiveApp")["ClientId"],

                AllowedGrantTypes = { GrantType.ResourceOwnerPassword },
                AllowOfflineAccess = true,

                ClientSecrets = { new Secret(clientConfigs.GetSection("NonInteractiveApp")["Secret"].Sha256()) },

                AllowedScopes = new []{
                                        "sub",
                                        "accounts-api",
                                        "tenants-api",
                                        "accounttenantmembership-api",
                                        "roles-api"
                                        }
            },
            new Client
            {
                ClientId = clientConfigs.GetSection("M2M_clientApp")["ClientId"],

                AlwaysSendClientClaims = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                ClientClaimsPrefix = "",
                Claims = new List<ClientClaim> {
                    new ClientClaim(JwtClaimTypes.Role, "M2M_role")
                },
                ClientName = "M2M_client",
                AllowedGrantTypes = { GrantType.ClientCredentials},
                AllowOfflineAccess = true,

                ClientSecrets = { new Secret(clientConfigs.GetSection("M2M_clientApp")["Secret"].Sha256()) },
                AllowedScopes = new []{
                                        "sub",
                                        "accounts-api",
                                        "tenants-api",
                                        "accounttenantmembership-api",
                                        "roles-api"
                                        }
            }
        };
    }
}
