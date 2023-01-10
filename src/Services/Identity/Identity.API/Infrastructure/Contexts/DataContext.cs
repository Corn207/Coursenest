using APICommonLibrary.Options;
using Identity.API.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Identity.API.Infrastructure.Contexts;

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
		if (_databaseOptions.Value.Seeding)
		{
			builder.Entity<User>().HasData(
				new User() { Email = "one@gmail.com", UserId = 1, FullName = "Elizabeth", Created = DateTime.Now, LastModified = DateTime.Now, Gender = Gender.Female },
				new User() { Email = "two@gmail.com", UserId = 2, FullName = "Emily", Created = DateTime.Now, LastModified = DateTime.Now, Gender = Gender.Female },
				new User() { Email = "three@gmail.com", UserId = 3, FullName = "Emma", Created = DateTime.Now, LastModified = DateTime.Now, Gender = Gender.Female },
				new User() { Email = "four@gmail.com", UserId = 4, FullName = "Jessica", Created = DateTime.Now, LastModified = DateTime.Now, Gender = Gender.Female },
				new User() { Email = "five@gmail.comstd", UserId = 5, FullName = "Brian", Created = DateTime.Now, LastModified = DateTime.Now, Gender = Gender.Male },
				new User() { Email = "six@gmail.com", UserId = 6, FullName = "Christopher", Created = DateTime.Now, LastModified = DateTime.Now, Gender = Gender.Male },
				new User() { Email = "sevent@gmail.com", UserId = 7, FullName = "David", Created = DateTime.Now, LastModified = DateTime.Now, Gender = Gender.Male }
				);

            builder.Entity<Experience>().HasData(
                new Experience() { ExperienceId = 1, Name = " Hanoi University", Title = "Professor", Started = DateTime.Now, UserId = 1 },
                new Experience() { ExperienceId = 2, Name = " BBOLD Talent", Title = "Employee", Started = DateTime.Now, UserId = 2 },
                new Experience() { ExperienceId = 3, Name = " SLEEP TRADING COMPANY LIMITED", Title = "Director", Started = DateTime.Now, UserId = 3 },
                new Experience() { ExperienceId = 4, Name = " XYZ TECHNOLOGY COMPANY LIMITED", Title = "Manager", Started = DateTime.Now, UserId = 4 },
                new Experience() { ExperienceId = 5, Name = " ABC INVESTMENT COMPANY LIMITED", Title = "Employee", Started = DateTime.Now, UserId = 5 },
                new Experience() { ExperienceId = 6, Name = " Hanoi University of science and technology", Title = "Engineer", Started = DateTime.Now, UserId = 6 },
                new Experience() { ExperienceId = 7, Name = " Ho Chi Minh University", Title = "Bachelor", Started = DateTime.Now, UserId = 7 }
                );

            builder.Entity<InterestedTopic>().HasData(
                new InterestedTopic() { UserId = 1, TopicId = 1 },
                new InterestedTopic() { UserId = 3, TopicId = 2 },
                new InterestedTopic() { UserId = 2, TopicId = 3 },
                new InterestedTopic() { UserId = 3, TopicId = 3 },
                new InterestedTopic() { UserId = 1, TopicId = 4 },
                new InterestedTopic() { UserId = 5, TopicId = 2 },
                new InterestedTopic() { UserId = 7, TopicId = 1 }
                );

            builder.Entity<FollowedTopic>().HasData(
               new FollowedTopic() { UserId = 1, TopicId = 1 },
               new FollowedTopic() { UserId = 2, TopicId = 1 },
               new FollowedTopic() { UserId = 4, TopicId = 3 },
               new FollowedTopic() { UserId = 6, TopicId = 1 },
               new FollowedTopic() { UserId = 5, TopicId = 2 },
               new FollowedTopic() { UserId = 3, TopicId = 2 },
               new FollowedTopic() { UserId = 2, TopicId = 4 }
                );
        }
    }

	public DbSet<Avatar> Avatars { get; set; }
	public DbSet<Experience> Experience { get; set; }
	public DbSet<InterestedTopic> InterestedTopics { get; set; }
	public DbSet<FollowedTopic> FollowedTopics { get; set; }
	public DbSet<User> Users { get; set; }
}
