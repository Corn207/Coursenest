using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.API.Helper;

public class JwtTokenHelper
{
	public string SecurityKey { get; set; } = string.Empty;
	public IEnumerable<Claim>? Claims { get; set; }
	public DateTime Expiry { get; set; } = DateTime.Now;


	public string WriteToken()
	{
		var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey));
		var signingCreds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
		var securityToken = new JwtSecurityToken(
			signingCredentials: signingCreds,
			claims: Claims,
			expires: Expiry
		);
		return new JwtSecurityTokenHandler().WriteToken(securityToken);
	}
}
