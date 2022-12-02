using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace APICommonLibrary;
public static class AppExtensions
{
    public static TContext OverwriteDatabase<TContext>(this IServiceProvider service) where TContext : DbContext
    {
        using var scope = service.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TContext>();

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        return db;
    }

    public static TContext OverwriteDatabase<TContext>(this IServiceProvider service, Action<TContext> context) where TContext : DbContext
    {
        using var scope = service.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TContext>();

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        context.Invoke(db);
        return db;
    }
}
