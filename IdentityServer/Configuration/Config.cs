using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace IdentityServer.Configuration;

public static class Config
{
    private const string SampleApiScope = "sampleapiscope";
    private const string SampleAspNetCoreWebScope = "sampleaspnetscope";

    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope(SampleApiScope, "SampleApi Scope"),
            new ApiScope(SampleAspNetCoreWebScope, "Sample Asp. Net Web Scope"),
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            // machine-to-machine clients
            new Client
            {
                ClientId = "sampleapi",
                ClientSecrets = { new Secret("apisecret".Sha256()) },

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                // scopes that client has access to
                AllowedScopes = { SampleApiScope }
            },
            // interactive ASP.NET Core Web App
            new Client
            {
                ClientId = "aspnetcoreweb",
                ClientSecrets = { new Secret("aspnetwebsecret".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,
 
                // where to redirect after login
                RedirectUris = { "https://localhost:7195/signin-oidc" },
 
                // where to redirect after logout
                PostLogoutRedirectUris = { "https://localhost:7195/signout-callback-oidc" },
 
                //TODO BackChannelLogoutUri = "",
 
                AllowOfflineAccess = true,

                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    SampleAspNetCoreWebScope,
                },

                RefreshTokenUsage = TokenUsage.ReUse,
                RefreshTokenExpiration =  TokenExpiration.Sliding,
            },
        };
}