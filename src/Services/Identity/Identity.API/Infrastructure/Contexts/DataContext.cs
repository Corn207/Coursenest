using APICommonLibrary.Options;
using Identity.API.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Identity.API.Infrastructure.Contexts;

public class DataContext : DbContext
{
	private readonly IOptions<DatabaseOptions> _databaseOptions;

	public DataContext(
		DbContextOptions<DataContext> options,
		IOptions<DatabaseOptions> databaseOptions) : base(options)
	{
		_databaseOptions = databaseOptions;
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
