namespace Identity.API.DTOs;

public record UserProfileResponse(
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
    List<ExperienceResponse> Experiences,
    List<int> InterestedTopicIds,
    List<int> FollowedTopicIds
    );
