using System.Text.Json.Serialization;

namespace Library.API.Models;

public class Category
{
    public Category(string content)
    {
        Content = content;
    }

    public int CategoryId { get; set; }
    public string Content { get; set; }

    // Relationship
    public List<Subcategory> Subcategories { get; set; } = new();
}
