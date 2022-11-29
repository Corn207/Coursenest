namespace UserData.API.Models;

public class Comment
{
    public Comment(string content)
    {
        Content = content;
    }

    public int CommentId { get; set; }
    public string Content { get; set; }

    // Relationship
    public int OwnerUserId { get; set; }

    public int SubmissionId { get; set; }
    public Submission Submission { get; set; } = null!;
}
