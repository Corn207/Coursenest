using Identity.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Contexts;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> opts) : base(opts)
    {
    }

    public DbSet<Image> Images { get; set; }
    public DbSet<User> Users { get; set; }
}
