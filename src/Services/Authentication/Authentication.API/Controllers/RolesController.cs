using APICommonLibrary.Utilities.APIs;
using Authentication.API.DTOs;
using Authentication.API.Infrastructure.Contexts;
using Authentication.API.Infrastructure.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authentication.API.Controllers;

[Route("[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly DataContext _context;

	public RolesController(IMapper mapper, DataContext context)
	{
		_mapper = mapper;
		_context = context;
	}


	// GET: /roles
	[HttpGet]
	[Authorize(Roles = nameof(APICommonLibrary.Models.Role.Admin))]
	public async Task<ActionResult<IEnumerable<RoleResult>>> GetAll(
		[FromQuery] int userId)
	{
		var exists = await _context.Credentials
			.AnyAsync(x => x.UserId == userId);
		if (!exists)
			return NotFound($"UserId: {userId} does not exist.");

		var results = await _context.Roles
			.Where(x => x.CredentialUserId == userId)
			.ProjectTo<RoleResult>(_mapper.ConfigurationProvider)
			.ToListAsync();

		return results;
	}

	// GET: /roles/me
	[HttpGet("me")]
	[Authorize]
	public async Task<ActionResult<IEnumerable<RoleResult>>> GetAllMe()
	{
		var userId = GetUserId();

		var exists = await _context.Credentials
			.AnyAsync(x => x.UserId == userId);
		if (!exists)
			return NotFound($"UserId: {userId} does not exist.");

		var results = await _context.Roles
			.Where(x => x.CredentialUserId == userId)
			.ProjectTo<RoleResult>(_mapper.ConfigurationProvider)
			.ToListAsync();

		return results;
	}


	// PUT: /roles/5
	[HttpPut("{userId}")]
	[Authorize(Roles = nameof(APICommonLibrary.Models.Role.Admin))]
	public async Task<ActionResult> Update(
		[FromBody] SetRole dto)
	{
		var credential = await _context.Credentials
			.AsNoTracking()
			.Include(x => x.Roles)
			.FirstOrDefaultAsync(x => x.UserId == dto.UserId);
		if (credential == null)
			return NotFound($"UserId: {dto.UserId} does not exist.");

		var role = _mapper.Map<Role>(dto);
		role.CredentialUserId = dto.UserId;

		if (credential.Roles.Any(x => x.Type == dto.Type))
		{
			_context.Roles.Update(role);
		}
		else
		{
			_context.Roles.Add(role);
		}

		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(GetAll), new { dto.UserId }, null);
	}


	private int GetUserId()
	{
		return ClaimUtilities.GetUserId(User.Claims);
	}
}
