using Microsoft.EntityFrameworkCore;
namespace Authentication.API.Models;

[PrimaryKey(nameof(CredentialUserId), nameof(Type))]
public class Role
{
	public RoleType Type { get; set; }
	public DateTime Expiry { get; set; }

	// Relationship
	public int CredentialUserId { get; set; }
	public Credential Credential { get; set; } = null!;
}

public enum RoleType
{
	Student, Instructor, Publisher, Admin
}
