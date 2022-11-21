
using Heus.Auth;
using Microsoft.EntityFrameworkCore;

namespace Heus.Enroll.Migrations;
public class MigrationAuthDbContext : AuthDbContext
{
    public MigrationAuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }
}
