using System.ComponentModel.DataAnnotations;

namespace Authentication.API.Options;

#nullable disable

public class JwtOptions
{
	public const string SectionName = "Jwt";

	[Required]
	[RegularExpression(@"^[a-zA-Z0-9]{8,32}$",
		ErrorMessage = "Value for {0} must be ^[a-zA-Z0-9]{8,32}$.")]
	public string SecretKey { get; set; }

	[Required]
	[Range(1, int.MaxValue,
		ErrorMessage = "Value for {0} must be bigger than or equal to {1}.")]
	public int AccessTokenLifetime { get; set; }

	[Required]
	[Range(1, int.MaxValue,
		ErrorMessage = "Value for {0} must be bigger than or equal to {1}.")]
	public int RefreshTokenLifetime { get; set; }
}
