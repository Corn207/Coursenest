using Library.API.DTOs.Ratings;
using Library.API.Models;

namespace Library.API.DTOs.Courses;

public record CourseCardResponse(
    int CourseId,
    string Title,
    string Description,
    CourseTier Tier,
    ImageResponse Image,
    RatingStatResponse RatingStat
    );
