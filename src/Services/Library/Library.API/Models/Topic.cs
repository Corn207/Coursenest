using System.Text.Json.Serialization;

namespace Library.API.Models;

public class Topic
{
    public Topic(string content)
    {
        Content = content;
    }

    public int TopicId { get; set; }
    public string Content { get; set; }

    // Relationship
    public int SubcategoryId { get; set; }
    public Subcategory Subcategory { get; set; } = null!;
}
