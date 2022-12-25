using Library.API.DTOs.Units;
using Library.API.Models;

namespace Library.API.DTOs.Courses;

public record CourseResponse(
    int CourseId,
    string Title,
    string Description,
    string About,
    CourseTier Tier,
    int TopicId,
    int PublisherUserId,
    DateTime LastModified,
    IEnumerable<UnitResponse> Materials,
    IEnumerable<UnitResponse> Exams
    );
