using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Library.API.Models;

[Table("Units")]
public abstract class Unit
{
    public Unit(string title)
    {
        Title = title;
    }

    public int UnitId { get; set; }
    public string Title { get; set; }
    public int OrderIndex { get; set; }

    // Relationship
    public int LessonId { get; set; }
    public Lesson Lesson { get; set; } = null!;
}
