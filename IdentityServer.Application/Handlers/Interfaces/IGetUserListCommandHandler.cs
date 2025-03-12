using IdentityServer.Domain;

namespace ApiUser.Domain.Interfaces.Handlers;

public interface IGetUserListCommandHandler
{
    Task<List<User>> HandleAsync(CancellationToken cancellationToken);
}