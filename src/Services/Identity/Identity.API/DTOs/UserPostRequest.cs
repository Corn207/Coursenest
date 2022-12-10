namespace Identity.API.DTOs;

public record UserPostRequest(
    string Email,
    string FullName
    );
