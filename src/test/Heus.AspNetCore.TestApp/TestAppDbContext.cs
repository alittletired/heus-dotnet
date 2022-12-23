using Heus.AspNetCore.TestApp.Domain;
using Heus.Data;
using Microsoft.EntityFrameworkCore;

namespace Heus.AspNetCore.TestApp;

public class TestAppDbContext : DbContextBase<TestAppDbContext>
{
    public TestAppDbContext(DbContextOptions<TestAppDbContext> options) : base(options)
    {
    }
    public DbSet<Person> People => Set<Person>();
}
