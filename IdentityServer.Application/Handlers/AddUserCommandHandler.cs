using IdentityServer.Application.Commands;
using IdentityServer.Application.Handlers.Interfaces;
using IdentityServer.Database.Interfaces;
using IdentityServer.Domain;

namespace IdentityServer.Application.Handlers;

public class AddUserCommandHandler(IUnitOfWork UnitOfWork,
    ICustomerRepository UserRepository) : IAddUserCommandHandler
{
    public async Task<User> HandleAsync(AddUserCommand command, CancellationToken cancellationToken)
    {
        var user = new User
        {
            EMail = command.EMail,
            GivenName = command.GivenName,
            Surname = command.Surname,
            Age = command.Age,
            Country = command.Country,
            City = command.City,
            Street = command.Street,
            HouseNumber = command.HouseNumber,
            PostalCode = command.PostalCode,
            Password = command.Password
        };


        var response = await UserRepository.AddUserAsync(user, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
        return user;
    }
}