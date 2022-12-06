namespace Identity.API.DTOs;

public record InfoResponse(
    int UserId,
    string FullName,
    ImageResponse? Avatar
    );
