using APICommonLibrary.MessageBus.Commands;
using APICommonLibrary.MessageBus.Responses;
using APICommonLibrary.Models;
using APICommonLibrary.Utilities.APIs;
using Authentication.API.DTOs;
using Authentication.API.Infrastructure.Contexts;
using Authentication.API.Infrastructure.Entities;
using Authentication.API.Options;
using Authentication.API.Utilities;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
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
	private readonly DataContext _context;
	private readonly IOptions<JwtOptions> _jwtOptions;
	private readonly IRequestClient<CreateUser> _createUserClient;
	private readonly IRequestClient<CheckTopicIds> _checkTopicIdsClient;
	private readonly IRequestClient<CheckUserEmails> _checkUserEmailsClient;

	public AuthenticateController(
		IMapper mapper,
		DataContext context,
		IOptions<JwtOptions> jwtOptions,
		IRequestClient<CreateUser> createUserClient,
		IRequestClient<CheckTopicIds> checkTopicIdsClient,
		IRequestClient<CheckUserEmails> checkUserEmailsClient)
	{
		_mapper = mapper;
		_context = context;
		_jwtOptions = jwtOptions;
		_createUserClient = createUserClient;
		_checkTopicIdsClient = checkTopicIdsClient;
		_checkUserEmailsClient = checkUserEmailsClient;
	}

	// POST: /authenticate/register
	[HttpPost("register")]
	[AllowAnonymous]
	public async Task<ActionResult> Register(
		[FromBody] Register body)
	{
		var exists = await _context.Credentials
			.AnyAsync(x => x.Username == body.Username);
		if (exists)
			return Conflict("Username existed.");

		var checkTopicIdsRequest = new CheckTopicIds()
		{
			TopicIds = body.InterestedTopicIds
		};
		var checkTopicsResponse = await _checkTopicIdsClient
			.GetResponse<Existed, NotFound>(checkTopicIdsRequest);
		if (checkTopicsResponse.Is(out Response<NotFound>? notFoundResponse))
		{
			return NotFound(notFoundResponse!.Message.Message);
		}

		var createUserRequest = _mapper.Map<CreateUser>(body);
		var createUserresponse = await _createUserClient
			.GetResponse<Created, Existed>(createUserRequest);
		if (createUserresponse.Is(out Response<Existed>? existedResponse))
		{
			return Conflict(existedResponse!.Message);
		}
		if (!createUserresponse.Is(out Response<Created>? createdResponse) ||
			createdResponse == null)
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}

		var credential = _mapper.Map<Credential>(body);
		credential.UserId = createdResponse.Message.Id;

		_context.Credentials.Add(credential);
		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(Login), null, null);
	}

	// POST: /authenticate/login
	[HttpPost("login")]
	[AllowAnonymous]
	public async Task<ActionResult<TokensResult>> Login(
		[FromBody] Login body)
	{
		var credential = await _context.Credentials
			.AsNoTracking()
			.Include(x => x.Roles)
			.FirstOrDefaultAsync(x =>
				x.Username == body.Username &&
				x.Password == body.Password);
		if (credential == null)
			return BadRequest("Credential is invalid.");

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
	[Authorize]
	public async Task<ActionResult> Logout()
	{
		var userId = GetUserId();

		var affected = await _context.RefreshTokens
			.Where(x => x.CredentialUserId == userId)
			.ExecuteDeleteAsync();
		if (affected == 0) return NotFound();

		return Ok();
	}

	// POST: /authenticate/refresh
	[HttpPost("refresh")]
	[AllowAnonymous]
	public async Task<ActionResult<AccessTokenResult>> Refresh(
		[FromBody] string token)
	{
		var refreshToken = await _context.RefreshTokens
			.Include(x => x.Credential.Roles)
			.FirstOrDefaultAsync(x => x.Token == token);
		if (refreshToken == null) 
			return NotFound("RefreshToken does not exist.");

		if (refreshToken.Expiry <= DateTime.Now)
		{
			_context.RefreshTokens.Remove(refreshToken);

			await _context.SaveChangesAsync();
			return BadRequest("RefreshToken expired.");
		}

		(string content, DateTime expiry) = CreateAccessToken(refreshToken.CredentialUserId, refreshToken.Credential.Roles);

		var result = new AccessTokenResult()
		{
			AccessToken = content,
			AccessTokenExpiry = expiry
		};

		return result;
	}

	// PUT: /authenticate/reset-password
	[HttpPut("reset-password")]
	[Authorize(Roles = nameof(RoleTypes.Admin))]
	public async Task<ActionResult<string>> ResetPassword(
		[FromBody] int userId)
	{
		var credential = await _context.Credentials
			.FirstOrDefaultAsync(x => x.UserId == userId);
		if (credential == null) 
			return NotFound($"UserId: {userId} does not exist.");

		var newPassword = Guid.NewGuid().ToString("N")[..8];
		credential.Password = newPassword;

		await _context.SaveChangesAsync();

		return newPassword;
	}

	// PUT: /authenticate/forgot-password
	[HttpPut("forgot-password")]
	[AllowAnonymous]
	public async Task<ActionResult<string>> ForgotPassword(
		[FromBody] ForgotPassword body)
	{
		var credential = await _context.Credentials
			.FirstOrDefaultAsync(x => x.Username == body.Username);
		if (credential == null)
			return NotFound("Credential does not exist.");
		// TODO Add userId check and email check
		var request = new CheckUserEmails() { Emails = new[] { body.Email } };
		var response = await _checkTopicIdsClient.GetResponse<Existed, NotFound>(request);

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
	[Authorize]
	public async Task<ActionResult<AccessTokenResult>> ChangePassword(
		[FromBody] ChangePassword body)
	{
		var userId = GetUserId();

		var credential = await _context.Credentials
			.FirstOrDefaultAsync(x => x.UserId == userId);
		if (credential == null)
			return NotFound($"UserId: {userId} does not exist.");
		if (credential.Password != body.OldPassword)
			return BadRequest("Old password is invalid.");

		credential.Password = body.NewPassword;
		await _context.SaveChangesAsync();

		return NoContent();
	}


	private (string content, DateTime expiry) CreateAccessToken(
		int userId,
		IEnumerable<Role> roles)
	{
		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, userId.ToString())
		};

		var validRoles = roles.Where(x => x.Expiry > DateTime.Now);
		claims.AddRange(validRoles.Select(x => new Claim(ClaimTypes.Role, x.Type.ToString())));

		var maximumExpiry = DateTime.Now.AddMinutes(_jwtOptions.Value.AccessTokenLifetime);
		var finalExpiry = maximumExpiry;
		if (validRoles.Any())
		{
			var nearestExpiry = validRoles.Min(x => x.Expiry);
			finalExpiry = (nearestExpiry - maximumExpiry) > TimeSpan.Zero ? maximumExpiry : nearestExpiry;
		}

		var helper = new JwtTokenHelper(_jwtOptions.Value.SecretKey, finalExpiry, claims);
		var content = helper.WriteToken();

		return (content, finalExpiry);
	}

	private int GetUserId()
	{
		return ClaimUtilities.GetUserId(User.Claims);
	}
}
