using IdentityServer.Domain;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Database;

public class IdentityServerContext : DbContext
{
    public IdentityServerContext(DbContextOptions<IdentityServerContext> options)
        : base(options)
    {

    }

    public DbSet<User> User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelbuilder)
    {
        modelbuilder.Entity<User>().HasData(new Domain.User
        {
            Id = 1,
            EMail = "igor@gmail.com",
            GivenName = "Igor",
            Surname = "Il",
            Age = 34,
            Country = "Germany",
            City = "Hamburg",
            Street = "Berner Chaussee",
            HouseNumber = 154,
            PostalCode = 22526,
            Password = "123456"
        });

        modelbuilder.Entity<User>().
            HasData(new Domain.User
            {
                Id = 2,
                EMail = "yury@gmail.com",
                GivenName = "Yury",
                Surname = "Spi",
                Age = 38,
                Country = "Germany",
                City = "Harburg",
                Street = "Harburger Chaussee",
                HouseNumber = 22,
                PostalCode = 22041,
                Password = "123456"
            });

        modelbuilder.Entity<User>().HasData(new Domain.User
        {
            Id = 3,
            EMail = "dirk@gmail.com",
            GivenName = "Dirk",
            Surname = "Es",
            Age = 33,
            Country = "Germany",
            City = "Berlin",
            Street = "Berliner Straße",
            HouseNumber = 232,
            PostalCode = 25014,
            Password = "123456"
        });
    }
}
