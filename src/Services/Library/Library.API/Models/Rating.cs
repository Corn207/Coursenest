using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Library.API.Models;

[PrimaryKey(nameof(CourseId), nameof(UserId))]
public class Rating
{
    public Rating(string content)
    {
        Content = content;
    }

    [Range(0, 5)]
    public int Stars { get; set; }
    public string Content { get; set; }
    public DateTime Created { get; set; }

    // Relationship
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;

    // External
    public int UserId { get; set; }
}
