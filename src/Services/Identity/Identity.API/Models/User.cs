namespace Identity.API.Models;

public class User
{
    public User(string email, string title, string aboutMe, string phonenumber, string location)
    {
        Email = email;
        Title = title;
        AboutMe = aboutMe;
        Phonenumber = phonenumber;
        Location = location;
    }

    public int UserId { get; set; }
    public string Email { get; set; }
    public string Title { get; set; }
    public string AboutMe { get; set; }
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Phonenumber { get; set; }
    public string Location { get; set; }

    // Relationship
    public int ImageId { get; set; }
    public Image Image { get; set; } = null!;

    public List<Experience> Experiences { get; set; } = new();

    public List<InterestedTopic> InterestedTopics { get; set; } = new();
}

public enum Gender
{
    Male, Female, Undefined
}
