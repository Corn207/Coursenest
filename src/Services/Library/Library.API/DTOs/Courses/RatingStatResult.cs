namespace Library.API.DTOs.Courses;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public record RatingStatResult
{
	public double AverageStars { get; set; }
	public int Total { get; set; }
}
