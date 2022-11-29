namespace UserData.API.Models;

public class Education
{
    public Education(string schoolName)
    {
        SchoolName = schoolName;
    }

    public int EducationId { get; set; }
    public string SchoolName { get; set; }
    public string? Degree { get; set; }
    public DateTime Started { get; set; }
    public DateTime? Ended { get; set; }

    // Relationship
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
