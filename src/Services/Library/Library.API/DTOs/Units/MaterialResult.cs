namespace Library.API.DTOs.Units;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record MaterialResult
{
	public int UnitId { get; set; }
	public string Title { get; set; }
	public TimeSpan RequiredTime { get; set; }
	public decimal Order { get; set; }
	public string Content { get; set; }
}
