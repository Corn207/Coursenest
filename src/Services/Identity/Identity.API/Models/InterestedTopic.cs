namespace Identity.API.Models;

public class InterestedTopic
{
    public int InterestedTopicId { get; set; }
    
    // Relationship
    public int TopicId { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
