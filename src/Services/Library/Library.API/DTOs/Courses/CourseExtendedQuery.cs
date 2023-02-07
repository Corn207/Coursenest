using CommonLibrary.API.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Library.API.DTOs.Courses;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record CourseExtendedQuery : CourseQuery
{
	public int? PublisherUserId { get; set; }
}
