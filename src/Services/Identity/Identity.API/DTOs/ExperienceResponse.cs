namespace Identity.API.DTOs;

public record ExperienceResponse(
    int ExperienceId,
    string Name,
    string Title,
    DateTime Started,
    DateTime? Ended
    );