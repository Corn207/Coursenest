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
	private readonly IRequestClient<GetTopic> _getTopicClient;

	public AuthenticateController(
		IMapper mapper,
		IOptions<JwtOptions> jwtOptions,
		DataContext context,
		IRequestClient<CreateUser> createUserClient,
		IRequestClient<GetTopic> getTopicClient)
	{
		_mapper = mapper;
		_jwtOptions = jwtOptions;
		_context = context;
		_createUserClient = createUserClient;
		_getTopicClient = getTopicClient;
	}

	// POST: /authenticate/register
	[HttpPost("register")]
	public async Task<ActionResult> Register(
		Register dto)
	{
		var exists = await _context.Credentials
			.AsNoTracking()
			.AnyAsync(x => x.Username == dto.Username);
		if (exists) return Conflict("Username existed.");

		foreach (var id in dto.InterestedTopicIds)
		{
			try
			{
				var getTopic = new GetTopic() { TopicId = id };
				var getTopicResponse = await _getTopicClient.GetResponse<GetTopicResult>(getTopic);
			}
			catch (KeyNotFoundException)
			{
				return NotFound($"TopicId ({id}) not existed.");
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}

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
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}

		var credential = _mapper.Map<Credential>(dto);
		credential.UserId = createUserResponse.Message.UserId;

		_context.Credentials.Add(credential);
		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(Login), null, null);
	}

	// POST: /authenticate/login
	[HttpPost("login")]
	public async Task<ActionResult<TokensResult>> Login(
		Login request)
	{
		var credential = await _context.Credentials
			.AsNoTracking()
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
			Expiry = refreshTokenExpiry,
			CredentialUserId = credential.UserId
		};

		_context.RefreshTokens.Add(refreshToken);
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
	public async Task<ActionResult> Logout(
		[FromHeader] int userId)
	{
		var exist = await _context.Credentials
			.AsNoTracking()
			.AnyAsync(x => x.UserId == userId);
		if (!exist) return NotFound();

		await _context.RefreshTokens
			.Where(x => x.CredentialUserId == userId)
			.ExecuteDeleteAsync();

		return Ok();
	}

	// POST: /authenticate/refresh
	[HttpPost("refresh")]
	public async Task<ActionResult<AccessTokenResult>> Refresh(
		[FromBody] string token)
	{
		var refreshToken = await _context.RefreshTokens
			.FirstOrDefaultAsync(x => x.Token == token);
		if (refreshToken == null) return NotFound();
		if (refreshToken.Expiry <= DateTime.Now)
		{
			_context.RefreshTokens.Remove(refreshToken);
			await _context.SaveChangesAsync();
			return Unauthorized();
		}

		var roles = await _context.Roles
			.AsNoTracking()
			.Where(x => x.CredentialUserId == refreshToken.CredentialUserId)
			.ToListAsync();

		(string content, DateTime expiry) = CreateAccessToken(refreshToken.CredentialUserId, roles);

		var result = new AccessTokenResult()
		{
			AccessToken = content,
			AccessTokenExpiry = expiry
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
		claims.AddRange(validRoles.Select(x => new Claim("Roles", x.Type.ToString())));

		var maximumExpiry = DateTime.Now.AddMinutes(_jwtOptions.Value.AccessTokenLifetime);
		DateTime finalExpiry = maximumExpiry;
		if (validRoles.Any())
		{
			var nearestExpiry = validRoles.Min(x => x.Expiry);
			finalExpiry = (nearestExpiry - maximumExpiry) > TimeSpan.Zero ? maximumExpiry : nearestExpiry;
		}

		var helper = new JwtTokenHelper(_jwtOptions.Value.SecretKey, finalExpiry, claims);
		var content = helper.WriteToken();

		return (content, finalExpiry);
	}
}
