namespace UserData.API.Models;

public class Rating
{
    public Rating(string content)
    {
        Content = content;
    }

    public int RatingId { get; set; }
    public string Content { get; set; }
    public int Stars { get; set; }
    public DateTime Created { get; set; }

    // Relationship
    public int CourseId { get; set; }

    public int UserId { get; set; }
}
