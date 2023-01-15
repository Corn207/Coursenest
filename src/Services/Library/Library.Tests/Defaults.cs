using Library.API.Infrastructure.Contexts;
using Library.API.Infrastructure.Entities;

namespace Library.Tests;

public static class Defaults
{
	public static readonly Action<DataContext> CategoriesDatabase = context =>
	{
		context.AddRange(new[]
		{
			new Category()
			{
				CategoryId = 1,
				Content = "Development",
				Subcategories = new()
				{
					new Subcategory()
					{
						Content = "Web Development",
						Topics = new()
						{
							new Topic() { Content = "JavaScript" },
							new Topic() { Content = "CSS" }
						}
					},
					new Subcategory()
					{
						Content = "Data Science",
						Topics = new()
						{
							new Topic() { Content = "Python" },
							new Topic() { Content = "Machine Learning" }
						}
					}
				}
			},
			new Category()
			{
				CategoryId = 2,
				Content = "Business",
				Subcategories = new()
				{
					new Subcategory()
					{
						Content = "Entrepreneurship",
						Topics = new()
						{
							new Topic() { Content = "Business Fundamentals" },
							new Topic() { Content = "Freelancing" }
						}
					},
					new Subcategory()
					{
						Content = "Communication",
						Topics = new()
						{
							new Topic() { Content = "Communication" },
							new Topic() { Content = "Public Speaking" }
						}
					}
				}
			},
			new Category() { CategoryId = 3, Content = "Finance & Accounting" },
			new Category() { CategoryId = 4, Content = "IT & Software" }
		});
	};

	public static readonly Action<DataContext> Database = context =>
	{
		context.AddRange(new[]
		{
			new Course()
		});
	};
}
