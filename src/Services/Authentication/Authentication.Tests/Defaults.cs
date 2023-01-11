using Authentication.API.Infrastructure.Contexts;
using Authentication.API.Infrastructure.Entities;

namespace Authentication.Tests;

public static class Defaults
{
	public static readonly Action<DataContext> Database = context =>
	{
		var creds = new List<Credential>()
		{
			new Credential() { Username = "usrbasic", Password = "pwd", UserId = 1 },
			new Credential() { Username = "usrstd", Password = "pwd", UserId = 2 },
			new Credential() { Username = "usrins", Password = "pwd", UserId = 3 },
			new Credential() { Username = "usrpub", Password = "pwd", UserId = 4 },
			new Credential() { Username = "usrad", Password = "pwd", UserId = 5 },
			new Credential() { Username = "usrnonad", Password = "pwd", UserId = 6 },
			new Credential() { Username = "usrfull", Password = "pwd", UserId = 7 }
		};
		context.Credentials.AddRange(creds);

		var roles = new List<Role>()
		{
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
		};
		context.Roles.AddRange(roles);
	};
}
