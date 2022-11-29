namespace UserData.API.Models;

public class InstructorFollow
{
    public int InstructorFollowId { get; set; }

    // Relationship
    public int InstructorUserId { get; set; }

    public List<int> FollowingTopics { get; set; } = new();

    public List<int> FollowingCourses { get; set; } = new();
}
