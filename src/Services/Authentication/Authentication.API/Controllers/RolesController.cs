using Authentication.API.DTOs;
using Authentication.API.Infrastructure.Contexts;
using Authentication.API.Infrastructure.Entities;
using AutoMapper;
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
			var exist = await _context.Credentials
				.AsNoTracking()
				.AnyAsync(x => x.UserId == userId);
			if (!exist) return NotFound();

			var results = await _context.Roles
				.AsNoTracking()
				.Where(x => x.CredentialUserId == userId)
				.Select(x => _mapper.Map<RoleResult>(x))
				.ToListAsync();

			return Ok(results);
		}

		// GET: /roles/me
		[HttpGet("me")]
		public async Task<ActionResult<IEnumerable<RoleResult>>> GetAllMe(
			[FromHeader] int userId)
		{
			var exist = await _context.Credentials
				.AsNoTracking()
				.AnyAsync(x => x.UserId == userId);
			if (!exist) return NotFound();

			var results = await _context.Roles
				.AsNoTracking()
				.Where(x => x.CredentialUserId == userId)
				.Select(x => _mapper.Map<RoleResult>(x))
				.ToListAsync();

			return Ok(results);
		}


		// PUT: /roles/5
		[HttpPut("{userId}")]
		public async Task<ActionResult> Update(
			int userId,
			SetRole dto)
		{
			var exist = await _context.Credentials
				.AsNoTracking()
				.AnyAsync(x => x.UserId == userId);
			if (!exist) return NotFound();

			var result = _mapper.Map<Role>(dto);
			result.CredentialUserId = userId;

			exist = await _context.Roles
				.AsNoTracking()
				.AnyAsync(x => x.CredentialUserId == userId && x.Type == result.Type);
			if (!exist)
			{
				_context.Roles.Add(result);
			}
			else
			{
				_context.Update(result);
			}
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetAll), userId, null);
		}
	}
}
