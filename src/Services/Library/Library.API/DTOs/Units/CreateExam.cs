namespace Library.API.DTOs.Units;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record CreateExam
{
	public string Title { get; set; }
	public int RequiredTimeMinutes { get; set; }
	public int LessonId { get; set; }
}
