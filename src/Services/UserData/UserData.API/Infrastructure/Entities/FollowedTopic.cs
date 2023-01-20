namespace UserData.API.Infrastructure.Entities;

public class FollowedTopic
{
    public int FollowedTopicId { get; set; }

    //Relationship
    public int TopicId { get; set; }
    public int UserId { get; set; }
}
