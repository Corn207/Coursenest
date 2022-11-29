namespace UserData.API.Models;

public class Checkpoint
{
    public Checkpoint(string content)
    {
        Content = content;
    }

    public int CheckpointId { get; set; }
    public string Content { get; set; }
    public int Point { get; set; }
    public bool IsChecked { get; set; }

    // Relationship
    public int CriterionId { get; set; }
    public Criterion Criterion { get; set; } = null!;
}
