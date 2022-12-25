using APICommonLibrary.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace APICommonLibrary;
public static class AppExtensions
{
	public static void Startup(this IServiceProvider service)
	{
		using var scope = service.CreateScope();
		var context = scope.ServiceProvider.GetRequiredService<DbContext>();
		var databaseOptions = service.GetRequiredService<IOptions<DatabaseOptions>>();

		if (databaseOptions.Value.Overwrite)
		{
			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();
		}
		else if (databaseOptions.Value.Create)
		{
			context.Database.EnsureCreated();
		}
	}
}
