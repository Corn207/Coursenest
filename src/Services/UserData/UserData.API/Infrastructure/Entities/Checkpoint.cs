namespace UserData.API.Infrastructure.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class Checkpoint
{
    public int CheckpointId { get; set; }
    public string Content { get; set; }
    public int Point { get; set; }
    public bool IsChosen { get; set; }

    // Relationship
    public int CriterionId { get; set; }
    public Criterion Criterion { get; set; }
}
