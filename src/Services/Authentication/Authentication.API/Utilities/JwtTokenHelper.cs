using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.API.Utilities;

public class JwtTokenHelper
{
	public JwtTokenHelper(string key, DateTime expiry, IEnumerable<Claim>? claims = null)
	{
		SecurityKey = key;
		Claims = claims;
		Expiry = expiry;
	}

	public string SecurityKey { get; set; }
	public IEnumerable<Claim>? Claims { get; set; }
	public DateTime Expiry { get; set; }


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
