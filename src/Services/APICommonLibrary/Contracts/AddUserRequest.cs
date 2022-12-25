namespace APICommonLibrary.Contracts;

public record AddUserRequest(
	string Email,
	string Fullname,
	int[] InterestedTopicIds
	);
