using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Configuration;

internal static class HostingExtensions
{
    public static IServiceCollection ConfigureDuende(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<DataProtectionKeysDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("ApiOnlineShopWebDb"));
        });

        services.AddDataProtection()
            .PersistKeysToDbContext<DataProtectionKeysDbContext>();


        services.AddAntiforgery(options =>
        {
            options.Cookie.SameSite = SameSiteMode.Lax;
        });


        services.AddIdentityServer(options => { })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients);

        services.AddAuthentication();

        //IdentityModelEventSource.ShowPII = true;

        return services;
    }
}