using APICommonLibrary.Options;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Reflection;
using System.Text.RegularExpressions;

namespace APICommonLibrary.Extensions;
public static class BuilderExtensions
{
	public static IServiceCollection AddMinimalDefaultServices(
		this IServiceCollection services,
		IConfiguration configuration,
		Action<IBusRegistrationConfigurator>? busConfig = null)
	{
		services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
		services.AddCors(options =>
			options.AddDefaultPolicy(configure =>
			{
				configure.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
			})
		);

		services.AddOptionalOptions<MassTransitOptions>(configuration, options =>
		{
			services.AddMassTransit(x =>
			{
				x.UsingRabbitMq((context, config) =>
				{
					config.Host(options.Host);
					config.ConfigureEndpoints(context);
				});

				busConfig?.Invoke(x);
			});
		});

		return services;
	}

	public static IServiceCollection AddDefaultServices<TDbContext>(
		this IServiceCollection services,
		IConfiguration configuration,
		Action<IBusRegistrationConfigurator>? busConfig = null) where TDbContext : DbContext
	{
		services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
		services.AddCors(options =>
			options.AddDefaultPolicy(configure =>
			{
				configure.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
			})
		);

		services.AddAutoMapper(Assembly.GetCallingAssembly());

		services.AddOptionalOptions<DatabaseOptions>(configuration);

		var cnnStr = configuration["Database:ConnectionString"];
		if (!string.IsNullOrWhiteSpace(cnnStr))
		{
			services.AddDbContext<DbContext, TDbContext>(builder =>
			{
				builder.UseSqlServer(cnnStr, builder =>
				{
					//builder.EnableRetryOnFailure(1, TimeSpan.FromSeconds(3), null);
				});
			});
		}
		else
		{
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
		}

		services.AddOptionalOptions<MassTransitOptions>(configuration, options =>
		{
			services.AddMassTransit(x =>
			{
				x.UsingRabbitMq((context, config) =>
				{
					config.Host(options.Host);
					config.ConfigureEndpoints(context);
				});

				busConfig?.Invoke(x);
			});
		});

		return services;
	}

	public static IServiceCollection AddOptionalOptions<TOptions>(
		this IServiceCollection services,
		IConfiguration configuration,
		Action<TOptions>? action = null) where TOptions : class
	{
		string sectionName = Regex.Replace(typeof(TOptions).Name, @"Options$", string.Empty);
		IConfigurationSection section = configuration.GetSection(sectionName);
		if (section.Exists())
		{
			var options = section.Get<TOptions>();
			if (options != null)
			{
				services.AddOptions<TOptions>()
					.Bind(section)
					.ValidateDataAnnotations()
					.ValidateOnStart();

				action?.Invoke(options);
			}
		}

		return services;
	}

	public static IServiceCollection AddRequiredOptions<TOptions>(
		this IServiceCollection services,
		IConfiguration configuration,
		Action<TOptions>? action = null) where TOptions : class
	{
		string sectionName = Regex.Replace(typeof(TOptions).Name, @"Options$", string.Empty);
		IConfigurationSection section;
		try
		{
			section = configuration.GetRequiredSection(sectionName);
		}
		catch (Exception)
		{
			throw;
		}

		var instance = (TOptions)(Activator.CreateInstance(typeof(TOptions))
			?? throw new Exception($"Cannot create instance of type {typeof(TOptions)}."));
		var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
		var isValid = Validator.TryValidateObject(instance, new ValidationContext(instance), results);
		if (!isValid)
		{
			var errors = string.Join(" ", results.Select(x => x.ErrorMessage));
			throw new Exception(errors);
		}

		services.AddOptions<TOptions>()
			.Bind(section)
			.ValidateDataAnnotations()
			.ValidateOnStart();

		action?.Invoke(instance);

		return services;
	}


	public static IApplicationBuilder DatabaseStartup(this IApplicationBuilder app)
	{
		using var scope = app.ApplicationServices.CreateScope();
		var context = scope.ServiceProvider.GetService<DbContext>();
		var options = app.ApplicationServices.GetService<IOptions<DatabaseOptions>>();

		if (context != null && options != null)
		{
			if (options.Value.EnsureDeleted)
			{
				context.Database.EnsureDeleted();
			}

			if (options.Value.EnsureCreated)
			{
				context.Database.EnsureCreated();
			}
		}

		return app;
	}
}
