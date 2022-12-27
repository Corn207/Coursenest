namespace Library.API.DTOs.Courses;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public record RatingResult
{
	public int OwnerUserId { get; set; }
	public int Stars { get; set; }
	public string Content { get; set; }
	public DateTime Created { get; set; }
}
