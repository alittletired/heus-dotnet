using Heus.Data;
using Microsoft.EntityFrameworkCore;
namespace Heus.Business;
public class EnrollDbContext : DbContextBase<EnrollDbContext>
{
    public EnrollDbContext(DbContextOptions<EnrollDbContext> options) : base(options)
    {
    }
}