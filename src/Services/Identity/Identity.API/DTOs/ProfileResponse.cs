namespace Identity.API.DTOs;

public record ProfileResponse(
    int UserId,
    string Email,
    string Title,
    string AboutMe,
    string Gender,
    DateTime DateOfBirth,
    string Phonenumber,
    string Location,
    string AvartarURI,
    List<ExperienceDTO> Experiences,
    List<int> InterestedTopicIds
    );