using Authentication.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Authentication.API.Contexts;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> opts) : base(opts)
    {
    }

    public DbSet<Credential> Credentials { get; set; }
<<<<<<< HEAD
    public DbSet<Role> Roles { get; set; }
=======
>>>>>>> 8f2d456107893510f74a5d3eedbdad6da5b6fe3d
}
