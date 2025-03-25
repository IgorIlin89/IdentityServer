using IdentityServer.Application.Commands;
using IdentityServer.Domain;

namespace IdentityServer.Application.Handlers.Interfaces;

public interface IAddUserCommandHandler
{
    Task<User> HandleAsync(AddUserCommand command, CancellationToken cancellationToken);
}