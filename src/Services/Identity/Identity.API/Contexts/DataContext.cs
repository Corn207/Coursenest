using Identity.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Contexts;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> opts) : base(opts)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Avatar> Avatars { get; set; }
    public DbSet<Experience> Experience { get; set; }
    public DbSet<InterestedTopic> InterestedTopics { get; set; }
    public DbSet<FollowedTopic> FollowedTopics { get; set; }
}
