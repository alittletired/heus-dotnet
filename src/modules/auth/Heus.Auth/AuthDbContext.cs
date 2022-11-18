using Heus.Ddd.Data;

namespace Heus.Auth;

internal class AuthDbContext : DbContextBase<AuthDbContext>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Organ> Organs { get; set; }
    public DbSet<Resource> Resources { get; set; }
    public DbSet<ActionRight> ActionRights { get; set; }
    public DbSet<RoleActionRight> RolePermissions { get; set; }


    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
    //    configurationBuilder
    //.Properties<long>()
    //.HaveConversion<longConverter>().HaveMaxLength(24).AreUnicode(false);
        base.ConfigureConventions(configurationBuilder);
    }
}
