using System.Text.Json.Serialization;

namespace Library.API.Models;

public class Choice
{
    public Choice(string content)
    {
        Content = content;
    }

    public int ChoiceId { get; set; }
    public string Content { get; set; }
    public bool IsCorrect { get; set; }

    // Relationship
    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;
}
