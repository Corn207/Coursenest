namespace Library.API.DTOs.Courses;

public record CourseQuery
{
	public string? Title { get; set; }
	public int? TopicId { get; set; }
	public int? PublisherUserId { get; set; }
	public int? Top { get; set; } = 5;
}
