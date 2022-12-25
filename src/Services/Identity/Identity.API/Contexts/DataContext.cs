using APICommonLibrary.Options;
using Identity.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Identity.API.Contexts;

public class DataContext : DbContext
{
	private readonly IOptions<ConnectionOptions> _connectionOptions;
	private readonly IOptions<DatabaseOptions> _databaseOptions;

	public DataContext(IOptions<ConnectionOptions> connectionOptions, IOptions<DatabaseOptions> databaseOptions)
	{
		_connectionOptions = connectionOptions;
		_databaseOptions = databaseOptions;
	}

	protected override void OnConfiguring(DbContextOptionsBuilder builder)
	{
		builder.UseSqlServer(_connectionOptions.Value.Database);
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		if (_databaseOptions.Value.Seed)
		{
		}
	}

	public DbSet<Avatar> Avatars { get; set; }
    public DbSet<Experience> Experience { get; set; }
    public DbSet<InterestedTopic> InterestedTopics { get; set; }
    public DbSet<FollowedTopic> FollowedTopics { get; set; }
    public DbSet<User> Users { get; set; }
}
