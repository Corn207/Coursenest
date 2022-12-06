namespace Identity.API.DTOs;

public record InstructorResponse(
    int UserId,
    string FullName,
    string? Title,
    string? AboutMe,
    ImageResponse? Avatar
    );