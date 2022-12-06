namespace Identity.API.DTOs;

public record AddUserRequest(
    string Email,
    string FullName
    );
