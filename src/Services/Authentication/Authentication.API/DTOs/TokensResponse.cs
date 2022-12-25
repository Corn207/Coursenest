namespace Authentication.API.DTOs;

public record TokensResponse(
	string AccessToken,
	DateTime AccessTokenExpiry,
	string RefreshToken,
	DateTime RefreshTokenExpiry
	);
