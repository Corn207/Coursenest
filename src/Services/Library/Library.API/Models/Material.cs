namespace Library.API.Models;

public class Material : Unit
{
    public Material(
        string title,
        int orderIndex,
        bool isCompleted,
        string content,
        TimeSpan approximateTime) : base(title, orderIndex, isCompleted)
    {
        Content = content;
        ApproximateTime = approximateTime;
    }

    public string Content { get; set; }
    public TimeSpan ApproximateTime { get; set; }
}
