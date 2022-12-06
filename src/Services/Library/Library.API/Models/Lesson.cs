using System.Text.Json.Serialization;

namespace Library.API.Models;

public class Lesson
{
    public Lesson(string title, string description)
    {
        Title = title;
        Description = description;
    }

    public int LessonId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int OrderIndex { get; set; }

    // Relationship
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public List<Unit> Units { get; set; } = new();
}
