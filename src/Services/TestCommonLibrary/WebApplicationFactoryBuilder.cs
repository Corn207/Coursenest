using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;

namespace TestCommonLibrary;
public class WebApplicationFactoryBuilder
{
	private Action<IServiceCollection>? _sqliteInMemoryAction;
	private Action<IServiceCollection>? _massTransitAction;


	public WebApplicationFactoryBuilder AddSqliteInMemory<TDbContext>() where TDbContext : DbContext
	{
		_sqliteInMemoryAction = services =>
		{
			// Create open SqliteConnection so EF won't automatically close it.
			services.AddSingleton<DbConnection>(container =>
			{
				var connection = new SqliteConnection("Filename=:memory:");
				connection.Open();

				return connection;
			});

			services.AddDbContext<DbContext, TDbContext>((container, options) =>
			{
				var connection = container.GetRequiredService<DbConnection>();
				options.UseSqlite(connection);
			});
		};

		return this;
	}

	public WebApplicationFactoryBuilder AddMassTransitTestHarness(Action<IBusRegistrationConfigurator> config)
	{
		_massTransitAction = services =>
		{
			services.AddMassTransitTestHarness(config);
		};

		return this;
	}


	public async Task<WebApplicationFactory<TEntryPoint>> BuildAsync<TEntryPoint>() where TEntryPoint : class
	{
		var factory = new WebApplicationFactory<TEntryPoint>()
			.WithWebHostBuilder(config =>
			{
				config.ConfigureServices(services =>
				{
					_sqliteInMemoryAction?.Invoke(services);
					_massTransitAction?.Invoke(services);
				});

				config.UseEnvironment("Development");
			});

		if (_massTransitAction != null)
		{
			var harness = factory.Services.GetTestHarness();
			await harness.Start();
		}

		return factory;
	}
}
