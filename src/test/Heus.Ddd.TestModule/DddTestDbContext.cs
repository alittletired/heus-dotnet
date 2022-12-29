using Heus.Data;
using Heus.Ddd.TestModule.Domain;
using Microsoft.EntityFrameworkCore;

namespace Heus.Ddd.TestModule;

public class DddTestDbContext: DbContextBase<DddTestDbContext>
{
    public DddTestDbContext(DbContextOptions<DddTestDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users => Set<User>();
    public DbSet<UserAddress> UserAddresses => Set<UserAddress>();
    public DbSet<Address> Addresses => Set<Address>();
}