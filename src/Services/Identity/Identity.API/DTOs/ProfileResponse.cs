namespace Identity.API.DTOs;

public record ProfileResponse(
    int UserId,
    string Email,
    string? Phonenumber,
    string FullName,
    string? Title,
    string? AboutMe,
    string? Gender,
    DateTime? DateOfBirth,
    string? Location,
    ImageResponse? Avatar,
    List<ExperienceDTO> Experiences,
    List<int> InterestedTopicIds
    );
