namespace Identity.API.DTOs;

public record ExperienceDTO(
    int ExperienceId,
    string Name,
    string Title,
    DateTime Started,
    DateTime? Ended
    );