using System.Text.Json.Serialization;

namespace Library.API.Models;

public class Exam : Unit
{
    public Exam(string title) : base(title)
    {
    }

    public TimeSpan TimeLimit { get; set; }

    // Relationship
    public List<Question> Questions { get; set; } = new();
}
