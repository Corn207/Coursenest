namespace UserData.API.Models;

public class Work
{
    public Work(string companyName, string title)
    {
        CompanyName = companyName;
        Title = title;
    }

    public int WorkId { get; set; }
    public string CompanyName { get; set; }
    public string Title { get; set; }
    public TimeSpan ExperienceYears { get; set; }
}
