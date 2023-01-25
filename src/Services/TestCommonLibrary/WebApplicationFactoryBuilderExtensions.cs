using MassTransit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;

namespace TestCommonLibrary;
public static class WebApplicationFactoryBuilderExtensions
{
	public WebApplicationFactoryBuilder AddEFCoreTestServices<TDbContext>(
		IServiceCollection services) where TDbContext : DbContext
	{
		services.AddSingleton<DbConnection>(_ =>
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

		return this;
	}

	public WebApplicationFactoryBuilder AddMassTransitTestServices(
		this IServiceCollection services,
		IServiceCollection servicesa,
		Action<IBusRegistrationConfigurator> bus)
	{
		services.AddMassTransitTestHarness(bus);

		_massTransitAction = services =>
		{
			services.AddMassTransitTestHarness(bus);
		};

		return this;
	}



	public static async Task<WebApplicationFactory<TEntryPoint>> DatabaseInitializeAsync<TEntryPoint, TDbContext>(
		this WebApplicationFactory<TEntryPoint> factory,
		Action<TDbContext> action) where TEntryPoint : class where TDbContext : DbContext
	{
		using var scope = factory.Services.CreateScope();
		var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
		context.Database.EnsureCreated();

		action.Invoke(context);
		await context.SaveChangesAsync();

		return factory;
	}

}
