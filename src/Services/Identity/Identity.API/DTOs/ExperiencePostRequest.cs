namespace Identity.API.DTOs;

public record ExperiencePostRequest(
    string Name,
    string Title,
    DateTime Started,
    DateTime? Ended
    );