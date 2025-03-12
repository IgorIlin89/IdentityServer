using IdentityServer.Database.Interfaces;

namespace IdentityServer.Database;

public class UnitOfWork(IdentityServerContext DbContext) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await DbContext.SaveChangesAsync();
    }
}
