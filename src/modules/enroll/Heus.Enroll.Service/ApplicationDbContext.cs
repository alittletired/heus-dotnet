using Heus.Enroll.Service.Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Heus.Business;
public class ApplicationDbContext:DbContextBase<ApplicationDbContext>
{
    public DbSet<User> Users=> Set<User>();
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}