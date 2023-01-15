using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TestCommonLibrary;
public static class WebApplicationFactoryExtensions
{
	public static WebApplicationFactory<TEntryPoint> DatabaseInitialize<TEntryPoint>(
		this WebApplicationFactory<TEntryPoint> factory) where TEntryPoint : class
	{
		using var scope = factory.Services.CreateScope();
		var context = scope.ServiceProvider.GetRequiredService<DbContext>();
		context.Database.EnsureCreated();

		return factory;
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
