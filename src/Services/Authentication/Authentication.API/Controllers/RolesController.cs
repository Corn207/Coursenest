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


		// GET: /roles/5
		[HttpGet("{userId}")]
		public async Task<ActionResult<IEnumerable<RoleResult>>> GetAll(int userId)
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
		public async Task<ActionResult<IEnumerable<RoleResult>>> GetAllMe([FromHeader] int userId)
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


		// POST: /roles/5
		[HttpPost("{credentialUserId}")]
		public async Task<ActionResult> Set(SetRole dto)
		{
			var exist = await _context.Credentials
				.AsNoTracking()
				.AnyAsync(x => x.UserId == dto.CredentialUserId);
			if (!exist) return NotFound();

			var result = _mapper.Map<Role>(dto);
			_context.Update(result);

			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetAll), dto.CredentialUserId, result);
		}
	}
}
