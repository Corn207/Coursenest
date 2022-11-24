using System.Text.Json.Serialization;

namespace Library.API.Models;

public class Course
{
    public Course(string title, string description, string about, DateTime lastUpdated, CourseTier tier, int publisherUserId)
    {
        Title = title;
        Description = description;
        About = about;
        LastUpdated = lastUpdated;
        Tier = tier;
        PublisherUserId = publisherUserId;
    }

    public int CourseId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string About { get; set; }
    public DateTime LastUpdated { get; set; }
    public CourseTier Tier { get; set; }
    public byte[]? Image { get; set; }

    // Relationship
    public int? TopicId { get; set; }
    public Topic? Topic { get; set; }

    public int PublisherUserId { get; set; }

    public List<Lesson> Lessons { get; set; } = new();
}

public enum CourseTier
{
    Free, Premium
}
