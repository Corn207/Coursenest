namespace Library.API.DTOs.Units;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record CreateQuestion
{
	public string Content { get; set; }
	public int Point { get; set; }
	public int ExamUnitId { get; set; }
	public List<CreateChoice> Choices { get; set; }

	public record CreateChoice
	{
		public string Content { get; set; }
		public bool IsCorrect { get; set; }
	}
}
