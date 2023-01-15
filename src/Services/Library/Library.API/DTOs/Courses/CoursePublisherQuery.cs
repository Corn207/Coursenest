using System.ComponentModel.DataAnnotations;

namespace Library.API.DTOs.Courses;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record CoursePublisherQuery
{
	public string? Title { get; set; }

	[Range(0, int.MaxValue)]
	public int? Page { get; set; } = 0;

	[Range(1, int.MaxValue)]
	public int? PageSize { get; set; } = 5;
}
