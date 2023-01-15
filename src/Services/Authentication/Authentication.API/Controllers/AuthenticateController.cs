using APICommonLibrary.MessageBus.Commands;
using APICommonLibrary.MessageBus.Responses;
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
	private readonly IRequestClient<CheckTopics> _checkTopicsClient;
	private readonly IRequestClient<CheckUserEmails> _checkUserEmailsClient;

	public AuthenticateController(
		IMapper mapper,
		IOptions<JwtOptions> jwtOptions,
		DataContext context,
		IRequestClient<CreateUser> createUserClient,
		IRequestClient<CheckTopics> checkTopicsClient,
		IRequestClient<CheckUserEmails> checkUserEmailsClient)
	{
		_mapper = mapper;
		_jwtOptions = jwtOptions;
		_context = context;
		_createUserClient = createUserClient;
		_checkTopicsClient = checkTopicsClient;
		_checkUserEmailsClient = checkUserEmailsClient;
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

		var checkTopicsRequest = new CheckTopics() { TopicIds = dto.InterestedTopicIds };
		var checkTopicsResponse = await _checkTopicsClient.GetResponse<Existed, NotFound>(checkTopicsRequest);

		if (checkTopicsResponse.Is(out Response<NotFound>? notFoundResponse))
		{
			return NotFound(notFoundResponse!.Message.Message);
		}

		var createUserRequest = _mapper.Map<CreateUser>(dto);
		var createUserresponse = await _createUserClient.GetResponse<Created, Existed>(createUserRequest);

		if (createUserresponse.Is(out Response<Existed>? existedResponse))
		{
			return Conflict(existedResponse!.Message);
		}

		if (!createUserresponse.Is(out Response<Created>? createdResponse) || createdResponse == null)
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}

		var credential = _mapper.Map<Credential>(dto);
		credential.UserId = createdResponse.Message.Id;

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
		if (credential.Password != request.Password) return BadRequest();

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
			UserId = credential.UserId,
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
		var affected = await _context.RefreshTokens
			.Where(x => x.CredentialUserId == userId)
			.ExecuteDeleteAsync();
		if (affected == 0) return NotFound();

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

	// PUT: /authenticate/reset-password
	[HttpPut("reset-password")]
	public async Task<ActionResult<string>> ResetPassword(
		[FromBody] int userId)
	{
		var credential = await _context.Credentials
			.FirstOrDefaultAsync(x => x.UserId == userId);
		if (credential == null) return NotFound();

		var newPassword = Guid.NewGuid().ToString("N")[..8];
		credential.Password = newPassword;

		await _context.SaveChangesAsync();

		return newPassword;
	}

	// PUT: /authenticate/forgot-password
	[HttpPut("forgot-password")]
	public async Task<ActionResult<string>> ForgotPassword(
		[FromBody] ForgotPassword dto)
	{
		var credential = await _context.Credentials
			.FirstOrDefaultAsync(x => x.Username == dto.Username);
		if (credential == null) return NotFound();

		var request = new CheckUserEmails() { Emails = new[] { dto.Email } };
		var response = await _checkTopicsClient.GetResponse<Existed, NotFound>(request);

		if (response.Is(out Response<NotFound>? notFoundResponse))
		{
			return NotFound(notFoundResponse!.Message.Message);
		}

		var newPassword = Guid.NewGuid().ToString("N")[..8];
		credential.Password = newPassword;

		await _context.SaveChangesAsync();

		return newPassword;
	}

	// PUT: /authenticate/change-password
	[HttpPut("change-password")]
	public async Task<ActionResult<AccessTokenResult>> ChangePassword(
		[FromHeader] int userId,
		[FromBody] ChangePassword dto)
	{
		var credential = await _context.Credentials
			.FirstOrDefaultAsync(x => x.UserId == userId);
		if (credential == null) return NotFound();
		if (credential.Password != dto.OldPassword) return BadRequest();

		credential.Password = dto.NewPassword;
		await _context.SaveChangesAsync();

		return NoContent();
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
