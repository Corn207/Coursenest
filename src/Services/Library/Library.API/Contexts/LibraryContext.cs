using Library.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Contexts;

public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> opts) : base(opts)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Subcategory> Subcategories { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
}
