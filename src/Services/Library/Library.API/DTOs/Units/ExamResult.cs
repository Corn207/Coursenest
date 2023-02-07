namespace Library.API.DTOs.Units;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record ExamResult
{
	public int UnitId { get; set; }
	public int CourseId { get; set; }
	public string Title { get; set; }
	public TimeSpan RequiredTime { get; set; }
	public decimal Order { get; set; }
	public List<QuestionResult> Questions { get; set; }
}
