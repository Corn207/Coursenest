using Library.API.Infrastructure.Contexts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Library.Tests;
internal class DefaultWebApplicationFactory : WebApplicationFactory<Program>
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureAppConfiguration(context =>
		{

		});

		builder.ConfigureServices(services =>
		{
			var dbContextDescriptor = services.SingleOrDefault(
				d => d.ServiceType ==
					typeof(DbContextOptions<DataContext>));
			if (dbContextDescriptor != null)
				services.Remove(dbContextDescriptor);

			var dbConnectionDescriptor = services.SingleOrDefault(
				d => d.ServiceType ==
					typeof(DbConnection));
			if (dbConnectionDescriptor != null)
				services.Remove(dbConnectionDescriptor);

			// Create open SqliteConnection so EF won't automatically close it.
			services.AddSingleton<DbConnection>(container =>
			{
				var connection = new SqliteConnection("DataSource=:memory:");
				connection.Open();

				return connection;
			});

			services.AddDbContext<DataContext>((container, options) =>
			{
				var connection = container.GetRequiredService<DbConnection>();
				options.UseSqlite(connection);
			});
		});

		

		builder.UseEnvironment("Development");
	}
}
