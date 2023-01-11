using APICommonLibrary.Options;
using Authentication.API.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Authentication.API.Infrastructure.Contexts;

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
	}

	public DbSet<Credential> Credentials { get; set; }
	public DbSet<Role> Roles { get; set; }
	public DbSet<RefreshToken> RefreshTokens { get; set; }
}
