namespace Identity.API.Models;

public class User
{
    public User(string email, string fullName)
    {
        Email = email;
        FullName = fullName;
    }

    public int UserId { get; set; }
    public string Email { get; set; }
    public string? Phonenumber { get; set; }
    public string FullName { get; set; }
    public string? Title { get; set; }
    public string? AboutMe { get; set; }
    public Gender? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Location { get; set; }

    // Relationship
    public int? AvatarImageId { get; set; }
    public Image? AvatarImage { get; set; } = null!;

    public List<Experience> Experiences { get; set; } = new();

    public List<InterestedTopic> InterestedTopics { get; set; } = new();
}

public enum Gender
{
    Male, Female
}
