using APICommonLibrary.Options;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Text.RegularExpressions;

namespace APICommonLibrary.Extensions;
public static class BuilderExtensions
{
	public static IServiceCollection AddDefaultServices<TDbContext>(
		this IServiceCollection services,
		IConfiguration configuration,
		Action<IBusRegistrationConfigurator>? busConfig = null) where TDbContext : DbContext
	{
		services.AddAutoMapper(Assembly.GetCallingAssembly());

		services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

		services.AddDefaultOptions<ConnectionOptions>(configuration);
		services.AddDefaultOptions<DatabaseOptions>(configuration);

		var section = Regex.Replace(typeof(ConnectionOptions).Name, @"Options$", string.Empty);
		var connectionOptions = configuration.GetSection(section).Get<ConnectionOptions>()!;

		services.AddDbContext<DbContext, TDbContext>(options =>
		{
			options.UseSqlServer(connectionOptions.Database, builder =>
			{
				//builder.EnableRetryOnFailure(1, TimeSpan.FromSeconds(3), null);
			});
		});

		services.AddMassTransit(x =>
		{
			x.UsingRabbitMq((context, config) =>
			{
				config.Host(connectionOptions.MessageBus);
				config.ConfigureEndpoints(context);
			});

			busConfig?.Invoke(x);
		});

		return services;
	}

	public static IApplicationBuilder DatabaseStartup(this IApplicationBuilder app)
	{
		var services = app.ApplicationServices;
		using var scope = services.CreateScope();
		var context = scope.ServiceProvider.GetRequiredService<DbContext>();
		var databaseOptions = services.GetRequiredService<IOptions<DatabaseOptions>>();

		if (databaseOptions.Value.Overwrite)
		{
			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();
		}
		else if (databaseOptions.Value.Create)
		{
			context.Database.EnsureCreated();
		}

		return app;
	}
}
