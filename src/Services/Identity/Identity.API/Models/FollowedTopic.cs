using Microsoft.EntityFrameworkCore;

namespace Identity.API.Models;

[PrimaryKey(nameof(UserId), nameof(TopicId))]
public class FollowedTopic
{
    // Relationship
    public int TopicId { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
