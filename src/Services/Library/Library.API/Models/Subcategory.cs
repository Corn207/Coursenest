using System.Text.Json.Serialization;

namespace Library.API.Models;

public class Subcategory
{
    public Subcategory(string content)
    {
        Content = content;
    }

    public int SubcategoryId { get; set; }
    public string Content { get; set; }

    // Relationship
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public List<Topic> Topics { get; set; } = new();
}
