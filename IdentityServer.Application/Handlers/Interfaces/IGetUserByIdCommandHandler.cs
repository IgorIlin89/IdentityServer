using IdentityServer.Application.Commands;
using IdentityServer.Domain;

namespace IdentityServer.Application.Handlers.Interfaces;

public interface IGetUserByIdCommandHandler
{
    Task<User?> HandleAsync(GetUserByIdCommand command, CancellationToken cancellationToken);
}