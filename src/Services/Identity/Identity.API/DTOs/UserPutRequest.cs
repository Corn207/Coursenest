namespace Identity.API.DTOs;

public record UserPutRequest(
    string? Email,
    string? Phonenumber,
    string? FullName,
    string? Title,
    string? AboutMe,
    string? Gender,
    DateTime? DateOfBirth,
    string? Location
    );
