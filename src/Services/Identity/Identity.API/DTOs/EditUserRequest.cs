namespace Identity.API.DTOs;

public record EditUserRequest(
    string? Email,
    string? Phonenumber,
    string? FullName,
    string? Title,
    string? AboutMe,
    string? Gender,
    DateTime? DateOfBirth,
    string? Location
    );
