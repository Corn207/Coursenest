using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Authentication.API.Infrastructure.Contexts;

public class DataContextDesignTimeFactory : IDesignTimeDbContextFactory<DataContext>
{
	public DataContext CreateDbContext(string[] args)
	{
		if (args.Length == 0 || string.IsNullOrWhiteSpace(args[0]))
		{
			throw new ArgumentException("Missing SQL Server ConnectionString in arguments.");
		}

		var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
		optionsBuilder.UseSqlServer(args[0]);

		return new DataContext(optionsBuilder.Options);
	}
}
