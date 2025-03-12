using IdentityServer.Application.Commands;
using IdentityServer.Application.Handlers.Interfaces;
using IdentityServer.Database.Interfaces;
using IdentityServer.Domain;

namespace IdentityServer.Application.Handlers;

public class GetUserByIdCommandHandler(ICustomerRepository Repository) : IGetUserByIdCommandHandler
{

    //if (id is null)
    //    {
    //        throw new NotFoundException($"The id may not be null when calling 'GetUserById'");
    //    }
    public async Task<User?> HandleAsync(GetUserByIdCommand command, CancellationToken cancellationToken)
        => await Repository.GetUserByIdAsync(int.Parse(command.UserId), cancellationToken);

}
