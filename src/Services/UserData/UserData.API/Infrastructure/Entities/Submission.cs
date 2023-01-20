namespace UserData.API.Infrastructure.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class Submission
{
	public int SubmissionId { get; set; }
	public string Title { get; set; }
	public string LessonName { get; set; }
	public string CourseName { get; set; }
	public DateTime Created { get; set; }
	public DateTime? Graded { get; set; }
	public TimeSpan? Elapsed { get; set; }
	public TimeSpan TimeLimit { get; set; }


	// Relationship
	public int StudentUserId { get; set; }

	public int InstructorUserId { get; set; }

	public List<Question> Questions { get; set; }

	public List<Criterion> Criteria { get; set; }

	public List<Comment> Comments { get; set; }
}
