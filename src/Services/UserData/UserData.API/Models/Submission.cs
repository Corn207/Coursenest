namespace UserData.API.Models;

public class Submission
{
    public Submission(string title, string lessonName, string courseName)
    {
        Title = title;
        LessonName = lessonName;
        CourseName = courseName;
    }

    public int SubmissionId { get; set; }
    public string Title { get; set; }
    public string LessonName { get; set; }
    public string CourseName { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Graded { get; set; }
    public TimeSpan Elapsed { get; set; }
    public TimeSpan TimeLimit { get; set; }


    // Relationship
    public int StudentUserId { get; set; }

    public int InstructorUserId { get; set; }

    public List<Question> Questions { get; set; } = new();

    public List<Criterion> Criteria { get; set; } = new();

    public List<Comment> Comments { get; set; } = new();
}
