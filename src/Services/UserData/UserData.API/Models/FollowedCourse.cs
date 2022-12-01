namespace UserData.API.Models;

public class FollowedCourse
{
    public int FollowedCourseId { get; set; }

    //Relationship
    public int CourseId { get; set; }
    public int UserId { get; set; }
}
