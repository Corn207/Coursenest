using Microsoft.EntityFrameworkCore;
using UserData.API.Models;

namespace UserData.API.Contexts;

public class DataContext : DbContext
{
	public DataContext(DbContextOptions<DataContext> opts) : base(opts)
	{
	}

	public DbSet<EnrolledCourse> EnrolledCourses { get; set; }
	public DbSet<CompletedUnit> CompletedUnits { get; set; }
	public DbSet<Submission> Submissions { get; set; }
	public DbSet<Criterion> Criteria { get; set; }
	public DbSet<Question> Questions { get; set; }
	public DbSet<Checkpoint> Checkpoints { get; set; }
	public DbSet<Answer> Answers { get; set; }
	public DbSet<FollowedCourse> FollowedCourses { get; set; }
	public DbSet<FollowedTopic> FollowedTopics { get; set; }
	public DbSet<Comment> Comments { get; set; }
}
