namespace Authentication.API.DTOs;

public record LoginRequest(
	string Username,
	string Password
	);
