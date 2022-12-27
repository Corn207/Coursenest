using APICommonLibrary.MessageBus.Commands;
using Authentication.API.DTOs;
using Authentication.API.Infrastructure.Contexts;
using Authentication.API.Infrastructure.Entities;
using Authentication.API.Options;
using Authentication.API.Utilities;
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
	private readonly IMapper _mapper;
	private readonly IOptions<JwtOptions> _jwtOptions;
	private readonly DataContext _context;
	private readonly IRequestClient<CreateUser> _createUserClient;

	public AuthenticateController(
		IMapper mapper,
		IOptions<JwtOptions> jwtOptions,
		DataContext context,
		IRequestClient<CreateUser> createUserClient)
	{
		_mapper = mapper;
		_jwtOptions = jwtOptions;
		_context = context;
		_createUserClient = createUserClient;
	}


	// POST: /authenticate/register
	[HttpPost("register")]
	public async Task<ActionResult> Register(Register dto)
	{
		var exist = await _context.Credentials
			.AsNoTracking()
			.AnyAsync(x => x.Username == dto.Username);
		if (exist) return Conflict("Username existed.");

		var createUser = _mapper.Map<CreateUser>(dto);
		Response<CreateUserResult> createUserResponse;
		try
		{
			createUserResponse = await _createUserClient.GetResponse<CreateUserResult>(createUser);
		}
		catch (ArgumentException)
		{
			return Conflict("Email existed.");
		}

		var result = _mapper.Map<Credential>(dto);
		result.UserId = createUserResponse.Message.UserId;

		_context.Credentials.Add(result);
		await _context.SaveChangesAsync();

		return StatusCode(201);
	}

	// POST: /authenticate/login
	[HttpPost("login")]
	public async Task<ActionResult<TokensResult>> Login(Login request)
	{
		var credential = await _context.Credentials
			.Include(x => x.Roles)
			.FirstOrDefaultAsync(x => x.Username == request.Username);
		if (credential == null) return NotFound();
		if (credential.Password != request.Password) return Unauthorized();

		(string accessTokenContent, DateTime accessTokenExpiry) = CreateAccessToken(credential.UserId, credential.Roles);

		string refreshTokenContent = Guid.NewGuid().ToString();
		var refreshTokenExpiry = DateTime.Now.AddMinutes(_jwtOptions.Value.RefreshTokenLifetime);

		var refreshToken = new RefreshToken()
		{
			Token = refreshTokenContent,
			Expiry = refreshTokenExpiry
		};

		credential.RefreshTokens.Add(refreshToken);
		await _context.SaveChangesAsync();

		var result = new TokensResult()
		{
			AccessToken = accessTokenContent,
			AccessTokenExpiry = accessTokenExpiry,
			RefreshToken = refreshTokenContent,
			RefreshTokenExpiry = refreshTokenExpiry
		};

		return result;
	}

	// POST: /authenticate/logout
	[HttpPost("logout")]
	public async Task<ActionResult> Logout([FromHeader] int userId)
	{
		var credential = await _context.Credentials
			.FindAsync(userId);
		if (credential == null) return NotFound();

		credential.RefreshTokens.Clear();
		await _context.SaveChangesAsync();

		return Ok();
	}

	// POST: /authenticate/refresh
	[Consumes("text/plain")]
	[HttpPost("refresh")]
	public async Task<ActionResult<AccessTokenResult>> Refresh(string refreshTokenContent)
	{
		var token = await _context.RefreshTokens
			.FindAsync(refreshTokenContent);
		if (token == null) return Unauthorized();
		if (token.Expiry <= DateTime.Now)
		{
			_context.RefreshTokens.Remove(token);
			await _context.SaveChangesAsync();
			return Unauthorized();
		}

		var roles = await _context.Roles
			.AsNoTracking()
			.Where(x => x.CredentialUserId == token.CredentialUserId)
			.ToListAsync();

		(string content, DateTime expiry) = CreateAccessToken(token.CredentialUserId, roles);

		var result = new AccessTokenResult()
		{
			Token = content,
			Expiry = expiry
		};

		return result;
	}


	private (string content, DateTime expiry) CreateAccessToken(int userId, IEnumerable<Role> roles)
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

		var helper = new JwtTokenHelper(_jwtOptions.Value.SecretKey, finalExpiry, claims);
		var content = helper.WriteToken();

		return (content, finalExpiry);
	}
}
