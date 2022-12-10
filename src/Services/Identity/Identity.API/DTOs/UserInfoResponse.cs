namespace Identity.API.DTOs;

public record UserInfoResponse(
    int UserId,
    string FullName,
    ImageResponse? Avatar
    );
