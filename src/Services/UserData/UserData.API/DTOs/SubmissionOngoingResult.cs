namespace UserData.API.DTOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record SubmissionOngoingResult
{
	public int SubmissionId { get; set; }
	public string Title { get; set; }
	public string LessonName { get; set; }
	public string CourseName { get; set; }
	public DateTime Created { get; set; }
	public TimeSpan TimeLimit { get; set; }

	// Relationship
	public int StudentUserId { get; set; }
	public int UnitId { get; set; }
	public int EnrollmentId { get; set; }
	public List<Question> Questions { get; set; }

	public record Question
	{
		public int QuestionId { get; set; }
		public string Content { get; set; }
		public int Point { get; set; }
		public List<Choice> Choices { get; set; }
	}

	public record Choice
	{
		public int ChoiceId { get; set; }
		public string Content { get; set; }
	}
}
