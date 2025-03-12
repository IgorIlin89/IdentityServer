using IdentityServer.Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Database;

public static class DbServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<IdentityServerContext>(configure =>
        {
            configure.UseSqlServer(configuration.GetConnectionString("ApiOnlineShopWebDb"), b => b.MigrationsAssembly("ApiUser"));
        });

        services.AddScoped<ICustomerRepository, CustomerRepository>()
            .AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
