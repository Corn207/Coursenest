namespace UserData.API.Infrastructure.Entities;

public class InstructorFollow
{
    public int InstructorFollowId { get; set; }

    // Relationship
    public int InstructorUserId { get; set; }

    public List<int> FollowingTopics { get; set; } = new();

    public List<int> FollowingCourses { get; set; } = new();
}
