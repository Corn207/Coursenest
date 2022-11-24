using System.Text.Json.Serialization;

namespace Library.API.Models;

public class Exam : Unit
{
    public Exam(
        string title,
        int orderIndex,
        bool isCompleted,
        TimeSpan timeLimit) : base(title, orderIndex, isCompleted)
    {
        TimeLimit = timeLimit;
    }

    public TimeSpan TimeLimit { get; set; }

    // Relationship
    public List<Question> Questions { get; set; } = new();
}
