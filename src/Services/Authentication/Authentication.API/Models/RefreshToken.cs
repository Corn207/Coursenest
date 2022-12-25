using System.ComponentModel.DataAnnotations;

namespace Authentication.API.Models;

public class RefreshToken
{
	public RefreshToken(string token)
	{
		Token = token;
	}

	[Key]
	public string Token { get; set; }
	public DateTime Expiry { get; set; }

	// Relationship
	public int CredentialUserId { get; set; }
	public Credential Credential { get; set; } = null!;
}
