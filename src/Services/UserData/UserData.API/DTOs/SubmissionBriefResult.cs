namespace UserData.API.DTOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record SubmissionBriefResult
{
	public int SubmissionId { get; set; }
	public string Title { get; set; }
	public string LessonName { get; set; }
	public string CourseName { get; set; }
	public DateTime Created { get; set; }
	public byte? Grade { get; set; }
	public DateTime? Graded { get; set; }
	public int UnitId { get; set; }
}
