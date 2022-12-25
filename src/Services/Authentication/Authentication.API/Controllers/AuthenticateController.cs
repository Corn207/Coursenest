using APICommonLibrary.Contracts;
using Authentication.API.Contexts;
using Authentication.API.DTOs;
using Authentication.API.Helper;
using Authentication.API.Models;
using Authentication.API.Options;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Authentication.API.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthenticateController : ControllerBase
{
	private readonly DataContext _context;
	private readonly IOptions<JwtOptions> _jwtOptions;
	private readonly IRequestClient<AddUserRequest> _addUserClient;
	private readonly IMapper _mapper;

	public AuthenticateController(
		DataContext context,
		IOptions<JwtOptions> jwtOptions,
		IRequestClient<AddUserRequest> addUserClient,
		IMapper mapper)
	{
		_context = context;
		_jwtOptions = jwtOptions;
		_addUserClient = addUserClient;
		_mapper = mapper;
	}


	// POST: /authenticate/register
	[HttpPost("register")]
	public async Task<ActionResult> Register(RegisterRequest request)
	{
		var addUserRequest = _mapper.Map<AddUserRequest>(request);

		Response<AddUserResponse> response;
		try
		{
			response = await _addUserClient.GetResponse<AddUserResponse>(addUserRequest);
		}
		catch (Exception)
		{
			return StatusCode(500);
		}

		var cred = new Credential(request.Username, request.Password)
		{
			UserId = response.Message.UserId
		};

		_context.Credentials.Add(cred);

		return Created("", request);
	}

	// POST: /authenticate/login
	[HttpPost("login")]
	public async Task<ActionResult> Login(LoginRequest request)
	{
		var cred = await _context.Credentials
			.FirstOrDefaultAsync(x => x.Username == request.Username && x.Password == request.Password);
		if (cred == null) return Unauthorized();

		await _context.Entry(cred)
			.Collection(x => x.Roles)
			.LoadAsync();

		(string accessToken, DateTime accessTokenExpiry) = CreateAccessToken(cred.UserId, cred.Roles);

		string refreshToken = Guid.NewGuid().ToString();
		var refreshTokenExpiry = DateTime.Now.AddMinutes(_jwtOptions.Value.RefreshTokenLifetime);
		cred.RefreshTokens.Add(new RefreshToken(refreshToken)
		{
			Expiry = refreshTokenExpiry
		});

		await _context.SaveChangesAsync();

		return Ok(new TokensResponse(accessToken, accessTokenExpiry, refreshToken, refreshTokenExpiry));
	}

	// POST: /authenticate/logout
	[HttpPost("logout")]
	public async Task<ActionResult> Logout([FromHeader] int UserId)
	{
		var affected = await _context.RefreshTokens
			.Where(x => x.CredentialUserId == UserId)
			.ExecuteDeleteAsync();

		return affected > 0 ? Ok() : Unauthorized();
	}

	// POST: /authenticate/refresh-token
	[HttpPost("refresh-token")]
	public async Task<ActionResult> RefreshToken(string refreshToken)
	{
		var token = await _context.RefreshTokens
			.FindAsync(refreshToken);
		if (token == null || token.Expiry < DateTime.Now) return Unauthorized();

		var roles = await _context.Roles
			.Where(x => x.CredentialUserId == token.CredentialUserId)
			.ToListAsync();

		(string accessToken, DateTime accessTokenExpiry) = CreateAccessToken(token.CredentialUserId, roles);

		return Ok(new TokensResponse(accessToken, accessTokenExpiry, refreshToken, token.Expiry));
	}


	private (string token, DateTime expiry) CreateAccessToken(int userId, List<Role> roles)
	{
		var claims = new List<Claim>
		{
			new Claim(JwtRegisteredClaimNames.Sub, userId.ToString())
		};

		var validRoles = roles.Where(x => x.Expiry > DateTime.Now);
		foreach (var role in validRoles)
		{
			claims.Add(new Claim("Roles", role.Type.ToString()));
		}

		var nearestExpiry = validRoles.Min(x => x.Expiry);
		var maximumExpiry = DateTime.Now.AddMinutes(_jwtOptions.Value.AccessTokenLifetime);
		var finalExpiry = (nearestExpiry - maximumExpiry) > TimeSpan.Zero ? maximumExpiry : nearestExpiry;

		var accessTokenHelper = new JwtTokenHelper()
		{
			SecurityKey = _jwtOptions.Value.SecretKey,
			Claims = claims,
			Expiry = finalExpiry
		};
		var accessToken = accessTokenHelper.WriteToken();

		return (accessToken, finalExpiry);
	}
}
