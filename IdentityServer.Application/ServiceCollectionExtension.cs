using ApiUser.Domain.Interfaces.Handlers;
using IdentityServer.Application.Handlers;
using IdentityServer.Application.Handlers.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Application;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        return serviceCollection.
            AddScoped<IGetUserListCommandHandler, GetUserListCommandHandler>().
            AddScoped<IGetUserByEmailCommandHandler, GetUserByEmailCommandHandler>().
            AddScoped<IGetUserByIdCommandHandler, GetUserByIdCommandHandler>().
            AddScoped<IAddUserCommandHandler, AddUserCommandHandler>();
    }
}
