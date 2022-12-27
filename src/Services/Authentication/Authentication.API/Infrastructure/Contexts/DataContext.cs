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
		if (_databaseOptions.Value.Seed)
		{
			builder.Entity<Credential>().HasData(
				new Credential() { Username = "usrbasic", Password = "pwd", UserId = 1 },
				new Credential() { Username = "usrstd", Password = "pwd", UserId = 2 },
				new Credential() { Username = "usrins", Password = "pwd", UserId = 3 },
				new Credential() { Username = "usrpub", Password = "pwd", UserId = 4 },
				new Credential() { Username = "usrad", Password = "pwd", UserId = 5 },
				new Credential() { Username = "usrnonad", Password = "pwd", UserId = 6 },
				new Credential() { Username = "usrfull", Password = "pwd", UserId = 7 }
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
