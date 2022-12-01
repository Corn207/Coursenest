namespace UserData.API.Models;

public class CompletedUnit
{
    public int CompletedUnitId { get; set; }
    
    // Relationship
    public int UnitId { get; set; }

    public int EnrolledCourseId { get; set; }
    public EnrolledCourse EnrolledCourse { get; set; } = null!;
}
