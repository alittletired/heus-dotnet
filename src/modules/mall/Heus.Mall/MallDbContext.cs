using Heus.Data;
using Microsoft.EntityFrameworkCore;
namespace Heus.Mall;
public class MallDbContext : DbContextBase<MallDbContext>
{
    public MallDbContext(DbContextOptions<MallDbContext> options) : base(options)
    {
    }
}