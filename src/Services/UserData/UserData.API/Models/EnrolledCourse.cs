namespace UserData.API.Models;

public class EnrolledCourse
{
    public int EnrolledCourseId { get; set; }
    public DateTime? CompletedDate { get; set; }

    // Relationship
    public int CourseId { get; set; }

    public int StudierUserId { get; set; }

<<<<<<< HEAD
    public List<CompletedUnit> CompletedUnitId { get; set; } = new();
=======
    public List<int> CompletedUnitId { get; set; } = new();
>>>>>>> 8f2d456107893510f74a5d3eedbdad6da5b6fe3d
}
