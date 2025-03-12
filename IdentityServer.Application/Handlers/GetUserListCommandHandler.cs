using ApiUser.Domain.Interfaces.Handlers;
using IdentityServer.Database.Interfaces;
using IdentityServer.Domain;

namespace IdentityServer.Application.Handlers;

public class GetUserListCommandHandler(IUnitOfWork UnitOfWork,
    ICustomerRepository Repository) : IGetUserListCommandHandler
{
    public async Task<List<User>> HandleAsync(CancellationToken cancellationToken)
        => await Repository.GetUserListAsync(cancellationToken);

}
