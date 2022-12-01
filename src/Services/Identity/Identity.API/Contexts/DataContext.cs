using Identity.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Contexts;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> opts) : base(opts)
    {
    }

<<<<<<< HEAD
    public DbSet<User> Users { get; set; }
    public DbSet<Experience> Experience { get; set; }
    public DbSet<InterestedTopic> InterestedTopics { get; set; }
    public DbSet<Image> Images { get; set; }
=======
    public DbSet<Image> Images { get; set; }
    public DbSet<User> Users { get; set; }
>>>>>>> 8f2d456107893510f74a5d3eedbdad6da5b6fe3d
}
