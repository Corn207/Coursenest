using APICommonLibrary.Options;
using Library.API.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Library.API.Infrastructure.Contexts;

public class DataContext : DbContext
{
	private readonly IOptions<DatabaseOptions> _databaseOptions;

	public DataContext(
		DbContextOptions<DataContext> options,
		IOptions<DatabaseOptions> databaseOptions) : base(options)
	{
		_databaseOptions = databaseOptions;
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.Entity<Lesson>()
			.Property(x => x.Order)
			.HasComputedColumnSql($"[{nameof(Lesson.OrderNumerator)}] / [{nameof(Lesson.OrderDenominator)}]", stored: true);

		builder.Entity<Unit>()
			.Property(x => x.Order)
			.HasComputedColumnSql($"[{nameof(Unit.OrderNumerator)}] / [{nameof(Unit.OrderDenominator)}]", stored: true);

		//crtl k u
		// = 5
		//builder.Entity<Category>().HasData(
		//	new Category()
		//	{
		//		CategoryId = 1,
		//		Content = "IT Development"
		//	},
		//	new Category()
		//	{
		//		CategoryId = 2,
		//		Content = "Medical"
		//	},
		//	new Category()
		//	{
		//		CategoryId = 3,
		//		Content = "Finance"
		//	});

		//// 3 per Category (Top 2) = 6
		//builder.Entity<Subcategory>().HasData(
		//	new Subcategory()
		//	{
		//		SubcategoryId = 1,
		//		Content = "IT Development"
		//	});

		//// 2 per Subcategory (Top 2) = 12
		//builder.Entity<Topic>().HasData(
		//	new Topic()
		//	{
		//		TopicId = 1,
		//		Content = "IT Development"
		//	});

		//// 3 per Topic (Top 2) (2 full) = 6
		//builder.Entity<Course>().HasData(
		//	new Course()
		//	{
		//		CourseId = 1,
		//		Title = "Learn Java",
		//		Description = "How to learn Java in 10 hours",
		//		About = "I will teach...",
		//		Tier = CourseTier.Free,
		//		IsApproved = false,
		//		Created = DateTime.Now,
		//		LastModified = DateTime.Now,
		//		TopicId = 1,
		//		PublisherUserId = 1
		//	},
		//	new Course()
		//	{
		//		CourseId = 2,
		//		Title = "Learn Java",
		//		Description = "How to learn Java in 10 hours",
		//		About = "I will teach...",
		//		Tier = CourseTier.Free,
		//		IsApproved = false,
		//		Created = DateTime.Now,
		//		LastModified = DateTime.Now,
		//		TopicId = 1,
		//		PublisherUserId = 1
		//	});

		//// 4 in Course 1 (2 full) + 2 in Course 2 = 6
		//builder.Entity<Lesson>().HasData(
		//	new Lesson("Basic Syntax", "How to write code")
		//	{
		//		LessonId = 1,
		//		OrderIndex = 1,
		//		CourseId = 1
		//	},
		//	new Lesson("Basic Concept", "How to read code")
		//	{
		//		LessonId = 2,
		//		OrderIndex = 2,
		//		CourseId = 1
		//	},
		//	new Lesson("HTML Basic", "Read HTML")
		//	{
		//		LessonId = 3,
		//		OrderIndex = 1,
		//		CourseId = 2
		//	});

		//// (2 Mat full > 1 Exam full > 1 Mat > 1 Exam full) = 5 in Lesson 1 Course 1
		//builder.Entity<Material>().HasData(
		//	new Material("Title", @"Longggggggggggggg...\ Doan van dai")
		//	{
		//		UnitId = 1,
		//		OrderIndex = 1,
		//		RequiredTime = TimeSpan.FromMinutes(15),
		//		LessonId = 1
		//	});

		//// (2 Mat full > 1 Exam full > 1 Mat > 1 Exam full) = 5 in Lesson 1 Course 1
		//builder.Entity<Exam>().HasData(
		//	new Exam("Title")
		//	{
		//		UnitId = 1,
		//		OrderIndex = 2,
		//		RequiredTime = TimeSpan.FromMinutes(15),
		//		LessonId = 1
		//	});

		//// (2 TN + 1 TL) per Exam full = 6
		//builder.Entity<Question>().HasData(
		//	new Question(@"Cau hoi 1: ABC?")
		//	{
		//		QuestionId = 1,
		//		Point = 2,
		//		ExamUnitId = 1
		//	});

		//// 3 per Question full = 12
		//builder.Entity<Choice>().HasData(
		//	new Choice(@"A. ABC")
		//	{
		//		ChoiceId = 1,
		//		IsCorrect = true,
		//		QuestionId = 1
		//	},
		//	new Choice(@"B. DEF")
		//	{
		//		ChoiceId = 2,
		//		IsCorrect = false,
		//		QuestionId = 1
		//	});

		//// 4 for Course 1, 2 for Course 2 = 6
		//builder.Entity<Rating>().HasData(
		//	new Rating()
		//	{
		//		Stars = 5,
		//		Content = @"Bai hoc nay bo ich (y).",
		//		Created = DateTime.Now,
		//		CourseId = 1,
		//		UserId = 1
		//	});
	}


	public DbSet<Category> Categories { get; set; }
	public DbSet<Subcategory> Subcategories { get; set; }
	public DbSet<Topic> Topics { get; set; }
	public DbSet<Course> Courses { get; set; }
	public DbSet<Lesson> Lessons { get; set; }
	public DbSet<Unit> Units { get; set; }
	public DbSet<Material> Materials { get; set; }
	public DbSet<Exam> Exams { get; set; }
	public DbSet<Question> Questions { get; set; }
	public DbSet<Choice> Choices { get; set; }
	public DbSet<CourseCover> CourseCovers { get; set; }
	public DbSet<Rating> Ratings { get; set; }
}
