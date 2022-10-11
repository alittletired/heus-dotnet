using Heus.Auth.Entities;
using Heus.Ddd.Data;
using Heus.Ddd.Data.ValueConversion;
using Heus.Ddd.Entities;
using Microsoft.EntityFrameworkCore;

namespace Heus.Auth
{
    internal class AuthDbContext : DbContextBase<AuthDbContext>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        public DbSet<Organ> Organs => Set<Organ>();
        public DbSet<Resource> Resources => Set<Resource>();

        public DbSet<RoleResource> RoleResources => Set<RoleResource>();


        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
        .Properties<EntityId>()
        .HaveConversion<EntityIdConverter>().HaveMaxLength(24).AreUnicode(false);
            base.ConfigureConventions(configurationBuilder);
        }
    }
}
