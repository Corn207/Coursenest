namespace Authentication.API.DTOs;

public record RegisterRequest(
	string Username,
	string Password,
	string Email,
	string Fullname,
	int[] InterestedTopicIds
	);
