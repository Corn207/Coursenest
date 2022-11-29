using System.Text.Json.Serialization;

namespace Library.API.Models;

public class Course
{
    public Course(string title, string description, string about)
    {
        Title = title;
        Description = description;
        About = about;
    }

    public int CourseId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string About { get; set; }
    public DateTime LastUpdated { get; set; }
    public CourseTier Tier { get; set; }
    public bool IsApproved { get; set; }

    // Relationship
    public int TopicId { get; set; }
    public Topic Topic { get; set; } = null!;

    public int PublisherUserId { get; set; }

    public Image Image { get; set; } = null!;

    public List<Lesson> Lessons { get; set; } = new();
}

public enum CourseTier
{
    Free, Premium
}
