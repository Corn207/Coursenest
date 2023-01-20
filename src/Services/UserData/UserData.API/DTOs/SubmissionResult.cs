namespace UserData.API.DTOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record SubmissionResult
{
	public int SubmissionId { get; set; }
	public string Title { get; set; }
	public string LessonName { get; set; }
	public string CourseName { get; set; }
	public DateTime Created { get; set; }
	public TimeSpan TimeLimit { get; set; }
	public TimeSpan Elapsed { get; set; }
	public int? Grade { get; set; }
	public DateTime? Graded { get; set; }
	public int StudentUserId { get; set; }
	public int? InstructorUserId { get; set; }
	public int UnitId { get; set; }
	public int EnrollmentId { get; set; }
	public List<Question> Questions { get; set; }
	public List<Criterion> Criteria { get; set; }
	public List<Comment> Comments { get; set; }


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
		public bool IsCorrect { get; set; }
		public bool? IsChosen { get; set; }
	}

	public record Criterion
	{
		public int CriterionId { get; set; }
		public string Content { get; set; }
		public List<Checkpoint> Checkpoints { get; set; }
	}

	public record Checkpoint
	{
		public int CheckpointId { get; set; }
		public string Content { get; set; }
		public int Point { get; set; }
		public bool IsChosen { get; set; }
	}

	public record Comment
	{
		public int CommentId { get; set; }
		public string Content { get; set; }
		public DateTime Created { get; set; }
		public int OwnerUserId { get; set; }
	}
}
