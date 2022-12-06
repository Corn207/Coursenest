namespace Identity.API.DTOs;

public record UserResponse(
    int UserId,
    string Email,
    string FullName
    );
