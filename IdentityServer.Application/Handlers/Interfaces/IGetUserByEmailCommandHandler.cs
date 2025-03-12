using IdentityServer.Application.Commands;
using IdentityServer.Domain;

namespace IdentityServer.Application.Handlers.Interfaces;

public interface IGetUserByEmailCommandHandler
{
    Task<User> HandleAsync(GetUserByEmailCommand command, CancellationToken cancellationToken);
}