using Library.API.Infrastructure.Entities;

namespace Library.API.DTOs.Courses;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record CourseResult
{
	public int CourseId { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public CourseTier Tier { get; set; }
	public ImageResult Cover { get; set; }
	public RatingStatResult RatingStat { get; set; }
}
