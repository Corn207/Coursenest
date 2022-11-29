namespace UserData.API.Models;

public class Criterion
{
    public Criterion(string content)
    {
        Content = content;
    }

    public int CriterionId { get; set; }
    public string Content { get; set; }

    // Relationship
    public int SubmissionId { get; set; }
    public Submission Submission { get; set; } = null!;

    public List<Checkpoint> Checkpoints { get; set; } = new();
}
