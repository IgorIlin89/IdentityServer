using IdentityServer.Database.Interfaces;
using IdentityServer.Domain;
using IdentityServer.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;


namespace IdentityServer.Database;

internal class CustomerRepository : ICustomerRepository
{
    private readonly IdentityServerContext _context;

    public CustomerRepository(IdentityServerContext context)
    {
        _context = context;
    }

    public async Task<User> AddUserAsync(User user, CancellationToken cancellationToken)
    {
        var existingUser = await _context.User.FirstOrDefaultAsync(o => o.EMail == user.EMail,
            cancellationToken);

        if (existingUser is not null)
        {
            throw new UserExistsException($"A user with the e-mail {user.EMail} is allready registered");
        }

        var response = await _context.User.AddAsync(new User
        {
            EMail = user.EMail,
            GivenName = user.GivenName,
            Surname = user.Surname,
            Age = user.Age,
            Country = user.Country,
            City = user.City,
            Street = user.Street,
            HouseNumber = user.HouseNumber,
            PostalCode = user.PostalCode,
            Password = user.Password
        },
        cancellationToken);

        return response.Entity;
    }

    public async Task<User> ChangePasswordAsync(int id, string password,
        CancellationToken cancellationToken)
    {
        var user = await _context.User.FirstOrDefaultAsync(o => o.Id == id,
            cancellationToken);

        if (user is null)
        {
            throw new NotFoundException($"Password not changed. User with id '{id}' could not be found");
        }

        user.Password = password;

        return user;
    }

    public async Task DeleteAsync(int id,
        CancellationToken cancellationToken)
    {
        var user = await _context.User.FirstOrDefaultAsync(o => o.Id == id,
            cancellationToken);

        if (user is not null)
        {
            _context.Remove(user);
        }
    }

    public async Task<User?> GetUserByEMailAsync(string email,
        CancellationToken cancellationToken)
    {
        var user = await _context.User.FirstOrDefaultAsync(o => o.EMail == email,
            cancellationToken);

        if (user is null)
        {
            throw new NotFoundException($"User with the email '{email}' does not exist");
        }

        return user;
    }

    public async Task<User?> GetUserByIdAsync(int id,
        CancellationToken cancellationToken)
    {
        var user = await _context.User.FirstOrDefaultAsync(o => o.Id == id,
            cancellationToken);

        if (user is null)
        {
            throw new NotFoundException($"User with the id '{id}' does not exist");
        }

        return user;
    }

    public async Task<List<User>> GetUserListAsync(CancellationToken cancellationToken)
    {
        return await _context.User.ToListAsync<User>(cancellationToken);
    }

    public async Task<User> UpdateAsync(User user,
        CancellationToken cancellationToken)
    {
        var userToEdit = await _context.User.FirstOrDefaultAsync(o => o.Id == user.Id,
            cancellationToken);

        if (userToEdit is null)
        {
            throw new NotFoundException($"User not updated. Could not find user with id '{user.Id}'");
        }

        userToEdit.EMail = user.EMail;
        userToEdit.GivenName = user.GivenName;
        userToEdit.Surname = user.Surname;
        userToEdit.Age = user.Age;
        userToEdit.Country = user.Country;
        userToEdit.City = user.City;
        userToEdit.Street = user.Street;
        userToEdit.HouseNumber = user.HouseNumber;
        userToEdit.PostalCode = user.PostalCode;
        userToEdit.Password = user.Password;

        return userToEdit;
    }
}
