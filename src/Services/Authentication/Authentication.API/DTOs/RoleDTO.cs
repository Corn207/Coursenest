using Authentication.API.Models;

namespace Authentication.API.DTOs;

public record RoleDTO(
	RoleType Type,
	DateTime Expiry
	);
