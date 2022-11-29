namespace Library.API.Models;

public class Material : Unit
{
    public Material(string title, string content) : base(title)
    {
        Content = content;
    }

    public string Content { get; set; }
    public TimeSpan ApproximateTime { get; set; }
}
