using IdentityServer.Application.Commands;
using IdentityServer.Application.Handlers.Interfaces;
using IdentityServer.Database.Interfaces;
using IdentityServer.Domain;

namespace IdentityServer.Application.Handlers;

public class GetUserByEmailCommandHandler(IUnitOfWork UnitOfWork,
    ICustomerRepository Repository) : IGetUserByEmailCommandHandler
{
    public async Task<User?> HandleAsync(GetUserByEmailCommand command, CancellationToken cancellationToken)
    {

        //if (string.IsNullOrWhiteSpace(email))
        //{
        //    throw new NotFoundException($"Email should not be null when searching for user by email");
        //}
        return await Repository.GetUserByEMailAsync(command.EMail, cancellationToken);
    }
}