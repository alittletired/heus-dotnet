using Heus.Auth.Entities;
using Microsoft.EntityFrameworkCore;

namespace Heus.Auth
{
    internal class AuthDbContext:DbContextBase
    {
        public DbSet<Organ> Organs => Set<Organ>();
        public DbSet<Resource> Resources => Set<Resource>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<RoleResource> RoleResources => Set<RoleResource>();

        public DbSet<User> Users => Set<User>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
    }
}
