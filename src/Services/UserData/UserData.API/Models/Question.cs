namespace UserData.API.Models;

public class Question
{
    public Question(string content)
    {
        Content = content;
    }

    public int QuestionId { get; set; }
    public string Content { get; set; }
    public int Point { get; set; }

    // Relationship
    public int SubmissionId { get; set; }
    public Submission Submission { get; set; } = null!;

    public List<Answer> Answers { get; set; } = new();
}
