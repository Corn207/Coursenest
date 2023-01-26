using System.Security.Claims;

namespace APICommonLibrary.Utilities.APIs;
public static class ClaimUtilities
{
	public static int GetUserId(IEnumerable<Claim> claims)
	{
		var value = claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

		return int.Parse(value);
	}
}
