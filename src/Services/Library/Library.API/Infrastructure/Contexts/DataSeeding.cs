using Library.API.Models;

namespace Library.API.Contexts;

public static class DataSeeding
{
    public static void Seeding(this DataContext context)
    {
        var categories = new[]
        {
            new Category("Development"),
            new Category("Business"),
            new Category("IT & Software"),
            new Category("Design")
        };

        categories[0].Subcategories.Add(new Subcategory("Web Development"));
        categories[0].Subcategories.Add(new Subcategory("Data Science"));

        categories[2].Subcategories.Add(new Subcategory("IT Certifications"));
        categories[2].Subcategories.Add(new Subcategory("Hardware"));

        categories[0].Subcategories[0].Topics.Add(new Topic("JavaScript"));
        categories[0].Subcategories[0].Topics.Add(new Topic("CSS"));
        categories[0].Subcategories[1].Topics.Add(new Topic("Python"));
        categories[0].Subcategories[1].Topics.Add(new Topic("Statistics"));
        categories[2].Subcategories[0].Topics.Add(new Topic("AWS Certification"));
        categories[2].Subcategories[0].Topics.Add(new Topic("Microsoft Certification"));

        var courses = new[]
        {
            new Course(
                "Learning hitting people",
                "This course will teach you how to cause damages",
                "Nah, too lazy too repeat",
                DateTime.Now,
                CourseTier.Free,
                1
            ) { Topic = categories[0].Subcategories[0].Topics[0] },
            new Course(
                "Learning petting people",
                "Healing people from acts of love",
                "*pat *pat...",
                DateTime.Now,
                CourseTier.Premium,
                1
            ) { Topic = categories[2].Subcategories[0].Topics[1] }
        };

        courses[0].Lessons.Add(new Lesson(
            "How to wind up your fist",
            "Literally the title said it all",
            false
            ));
        courses[0].Lessons.Add(new Lesson(
            "How to find your target",
            "Read title pls",
            false
            ));

        courses[0].Lessons[0].Units.Add(new Material(
            "Punch Guildline",
            0,
            false,
            "Abc...",
            TimeSpan.FromMinutes(5)
            ));
        courses[0].Lessons[0].Units.Add(new Exam(
            "Test your knowledge",
            0,
            false,
            TimeSpan.FromMinutes(10)
            ));

        var exam = (Exam)courses[0].Lessons[0].Units[1];
        exam.Questions.Add(new Question("Use what hand first?", 1));
        exam.Questions.Add(new Question("Aim to what part first?", 1));
        exam.Questions.Add(new Question("Describe the process", 3));

        exam.Questions[0].Answers = new()
        {
            new Answer("Left hand", false),
            new Answer("Right hand", true)
        };
        exam.Questions[1].Answers = new()
        {
            new Answer("Head", true),
            new Answer("Body", false),
            new Answer("Leg", false)
        };


        foreach (var item in categories)
        {
            context.Categories.Add(item);
        }

        foreach (var item in courses)
        {
            context.Courses.Add(item);
        }

        context.SaveChanges();
    }
}
