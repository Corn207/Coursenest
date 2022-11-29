namespace UserData.API.Models;

public class Answer
{
    public Answer(string content)
    {
        Content = content;
    }

    public int AnswerId { get; set; }
    public string Content { get; set; }
    public bool? IsCorrect { get; set; }
    public bool? IsChosen { get; set; }

    // Relationship
    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;
}
