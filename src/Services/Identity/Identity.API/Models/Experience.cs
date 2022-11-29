namespace Identity.API.Models;

public class Experience
{
    public Experience(string name, string title)
    {
        Name = name;
        Title = title;
    }

    public int ExperienceId { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public DateTime Started { get; set; }
    public DateTime? Ended { get; set; }

    // Relationship
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
