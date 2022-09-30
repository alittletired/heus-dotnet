using Heus.Auth.Entities;
using Heus.Ddd.Data;
using Microsoft.EntityFrameworkCore;

namespace Heus.Auth
{
    internal class AuthDbContext:DbContextBase<AuthDbContext>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        public DbSet<Organ> Organs => Set<Organ>();
        public DbSet<Resource> Resources => Set<Resource>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<RoleResource> RoleResources => Set<RoleResource>();

        public DbSet<User> Users => Set<User>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
    }
}
