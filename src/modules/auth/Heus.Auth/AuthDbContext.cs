using Heus.Ddd.Data;
namespace Heus.Auth;

internal class AuthDbContext : DbContextBase<AuthDbContext>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Organ> Organs => Set<Organ>();
    public DbSet<Resource> Resources => Set<Resource>();
    public DbSet<ActionRight> ActionRights => Set<ActionRight>();
    public DbSet<RoleActionRight> RolePermissions => Set<RoleActionRight>();


    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
    //    configurationBuilder
    //.Properties<long>()
    //.HaveConversion<longConverter>().HaveMaxLength(24).AreUnicode(false);
        base.ConfigureConventions(configurationBuilder);
    }
}
