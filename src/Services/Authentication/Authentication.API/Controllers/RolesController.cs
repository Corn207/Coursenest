using Authentication.API.DTOs;
using Authentication.API.Infrastructure.Contexts;
using Authentication.API.Infrastructure.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authentication.API.Controllers
{
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
		[HttpGet()]
		public async Task<ActionResult<IEnumerable<RoleResult>>> GetAll(
			[FromQuery] int userId)
		{
			var exists = await _context.Credentials
				.AsNoTracking()
				.AnyAsync(x => x.UserId == userId);
			if (!exists) return NotFound();

			var results = await _context.Roles
				.AsNoTracking()
				.Where(x => x.CredentialUserId == userId)
				.ProjectTo<RoleResult>(_mapper.ConfigurationProvider)
				.ToListAsync();

			return Ok(results);
		}

		// GET: /roles/me
		[HttpGet("me")]
		public async Task<ActionResult<IEnumerable<RoleResult>>> GetAllMe(
			[FromHeader] int userId)
		{
			var exists = await _context.Credentials
				.AsNoTracking()
				.AnyAsync(x => x.UserId == userId);
			if (!exists) return NotFound();

			var results = await _context.Roles
				.AsNoTracking()
				.Where(x => x.CredentialUserId == userId)
				.ProjectTo<RoleResult>(_mapper.ConfigurationProvider)
				.ToListAsync();

			return Ok(results);
		}


		// PUT: /roles/5
		[HttpPut("{userId}")]
		public async Task<ActionResult> Update(
			int userId,
			SetRole dto)
		{
			var credential = await _context.Credentials
				.Include(x => x.Roles)
				.FirstOrDefaultAsync(x => x.UserId == userId);
			if (credential == null) return NotFound();

			var role = _mapper.Map<Role>(dto);
			role.CredentialUserId = userId;

			if (credential.Roles.Any(x => x.Type == dto.Type))
			{
				_context.Roles.Update(role);
			}
			else
			{
				_context.Roles.Add(role);
			}

			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetAll), new { userId }, null);
		}
	}
}
