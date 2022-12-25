using Microsoft.AspNetCore.Mvc;

namespace Library.API.DTOs.Courses;

public record CoursesQueries(
    [FromQuery(Name = "TopicIds")] int[] TopicIds,
    int? PublisherUserId,
    int Top = 5
    );
