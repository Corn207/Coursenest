using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication.API.Models;

[Index(nameof(Username), IsUnique = true)]
public class Credential
{
	public Credential(string username, string password)
	{
		Username = username;
		Password = password;
	}

	public string Username { get; set; }
	public string Password { get; set; }

	// Relationship
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int UserId { get; set; }

	public List<Role> Roles { get; set; } = new();

	public List<RefreshToken> RefreshTokens { get; set; } = new();
}
