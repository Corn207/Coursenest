namespace Identity.API.DTOs;

public record UserInstructorResponse(
    int UserId,
    string FullName,
    string? Title,
    string? AboutMe,
    ImageResponse? Avatar
    );