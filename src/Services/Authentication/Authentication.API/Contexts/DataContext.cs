using APICommonLibrary.Options;
using Authentication.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Authentication.API.Contexts;

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
			builder.Entity<Credential>().HasData(
				new Credential("usrbasic", "pwd") { UserId = 1 },
				new Credential("usrstd", "pwd") { UserId = 2 },
				new Credential("usrins", "pwd") { UserId = 3 },
				new Credential("usrpub", "pwd") { UserId = 4 },
				new Credential("usrad", "pwd") { UserId = 5 },
				new Credential("usrnonad", "pwd") { UserId = 6 },
				new Credential("usrfull", "pwd") { UserId = 7 }
				);

			builder.Entity<Role>().HasData(
				new Role() { CredentialUserId = 2, Type = RoleType.Student, Expiry = DateTime.Now.AddHours(1) },
				new Role() { CredentialUserId = 3, Type = RoleType.Instructor, Expiry = DateTime.Now.AddHours(1) },
				new Role() { CredentialUserId = 4, Type = RoleType.Publisher, Expiry = DateTime.Now.AddHours(1) },
				new Role() { CredentialUserId = 5, Type = RoleType.Admin, Expiry = DateTime.Now.AddHours(1) },
				new Role() { CredentialUserId = 6, Type = RoleType.Student, Expiry = DateTime.Now.AddHours(1) },
				new Role() { CredentialUserId = 6, Type = RoleType.Instructor, Expiry = DateTime.Now.AddHours(1) },
				new Role() { CredentialUserId = 6, Type = RoleType.Publisher, Expiry = DateTime.Now.AddHours(1) },
				new Role() { CredentialUserId = 7, Type = RoleType.Student, Expiry = DateTime.Now.AddHours(1) },
				new Role() { CredentialUserId = 7, Type = RoleType.Instructor, Expiry = DateTime.Now.AddHours(1) },
				new Role() { CredentialUserId = 7, Type = RoleType.Publisher, Expiry = DateTime.Now.AddHours(1) },
				new Role() { CredentialUserId = 7, Type = RoleType.Admin, Expiry = DateTime.Now.AddHours(1) }
				);
		}
	}

	public DbSet<Credential> Credentials { get; set; }
	public DbSet<Role> Roles { get; set; }
	public DbSet<RefreshToken> RefreshTokens { get; set; }
}
