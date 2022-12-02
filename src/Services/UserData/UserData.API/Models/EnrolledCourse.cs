namespace UserData.API.Models;

public class EnrolledCourse
{
    public int EnrolledCourseId { get; set; }
    public DateTime? CompletedDate { get; set; }

    // Relationship
    public int CourseId { get; set; }

    public int StudierUserId { get; set; }

    public List<CompletedUnit> CompletedUnitId { get; set; } = new();
}
