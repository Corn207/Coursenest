using Authentication.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Authentication.API.Contexts;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> opts) : base(opts)
    {
    }

    public DbSet<Credential> Credentials { get; set; }
    public DbSet<Role> Roles { get; set; }
}
