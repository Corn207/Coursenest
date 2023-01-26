using UserData.API.Infrastructure.Entities;
using static UserData.API.Infrastructure.Entities.Review;

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
	public byte? Grade { get; set; }
	public DateTime? Graded { get; set; }
	public int StudentUserId { get; set; }
	public int? InstructorUserId { get; set; }
	public List<QuestionResult> Questions { get; set; }
	public List<ReviewResult> Reviews { get; set; }
	public List<CommentResult> Comments { get; set; }


	public record QuestionResult
	{
		public int QuestionId { get; set; }
		public string Content { get; set; }
		public int Point { get; set; }
		public List<ChoiceResult> Choices { get; set; }
	}

	public record ChoiceResult
	{
		public int ChoiceId { get; set; }
		public string Content { get; set; }
		public bool IsCorrect { get; set; }
		public bool? IsChosen { get; set; }
	}

	public record ReviewResult
	{
		public int ReviewId { get; set; }
		public string Content { get; set; }
		public ReviewType Type { get; set; }
	}

	public record CommentResult
	{
		public int CommentId { get; set; }
		public string Content { get; set; }
		public DateTime Created { get; set; }
		public int OwnerUserId { get; set; }
	}
}
