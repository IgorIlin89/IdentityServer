using IdentityServer.Domain;

namespace IdentityServer.Database.Interfaces;

public interface ICustomerRepository
{
    Task<User> AddUserAsync(User user, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task<User?> GetUserByIdAsync(int id, CancellationToken cancellationToken);
    Task<User?> GetUserByEMailAsync(string email, CancellationToken cancellationToken);
    Task<List<User>> GetUserListAsync(CancellationToken cancellationToken);
    Task<User> UpdateAsync(User user, CancellationToken cancellationToken);
    Task<User> ChangePasswordAsync(int id, string password, CancellationToken cancellationToken);
}