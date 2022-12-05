using Heus.Data;
namespace Heus.Auth;

public class AuthDbContext : DbContextBase<AuthDbContext>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles  => Set<Role>();
    public DbSet<UserRole> UserRoles  => Set<UserRole>();
    public DbSet<Organ> Organs  => Set<Organ>();
    public DbSet<Resource> Resources  => Set<Resource>();
    public DbSet<ActionRight> ActionRights  => Set<ActionRight>();
    public DbSet<RoleActionRight> RolePermissions  => Set<RoleActionRight>();


    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
    //    configurationBuilder
    //.Properties<long>()
    //.HaveConversion<longConverter>().HaveMaxLength(24).AreUnicode(false);
        base.ConfigureConventions(configurationBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
       // SeedUsers(modelBuilder);
      //  SeedRoles(modelBuilder);
    }

    private void SeedUsers(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        var adminUser = new User()
        {
            Name = "admin",
            Id = 1,
            IsSuperAdmin = true,
            NickName = "超级管理员",
            Phone = "13900000000"

        };
        adminUser.SetPassword("1");
        modelBuilder.Entity<User>().HasData(adminUser);
        
    }

    private void SeedRoles(ModelBuilder modelBuilder)
    {
        var roles = new List<Role>
        {
            new() { Id = 1,Name = "super_admin", Remarks = "超级管理员", IsBuildIn = true },
            new() { Id = 2,Name = "common_user", Remarks = "普通用户", IsBuildIn = true }
        };
        modelBuilder.Entity<Role>().HasData(roles);
    }
}
