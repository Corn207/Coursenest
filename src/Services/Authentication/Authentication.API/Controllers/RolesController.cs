using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Authentication.API.Contexts;
using Authentication.API.Models;
using Authentication.API.DTOs;
using AutoMapper;
using System.Composition;

namespace Authentication.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RolesController : ControllerBase
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public RolesController(DataContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}


		// GET: api/Roles/5
		[HttpGet("{userId}")]
		public async Task<ActionResult<IEnumerable<RoleDTO>>> GetRoles(int userId)
		{
			var cred = await _context.Credentials
				.FindAsync(userId);
			if (cred == null) return NotFound();

			await _context.Entry(cred)
				.Collection(x => x.Roles)
				.LoadAsync();

			return Ok(cred.Roles.Select(x => _mapper.Map<RoleDTO>(x)));
		}


		// PUT: api/Roles/5
		[HttpPut("{userId}")]
		public async Task<IActionResult> PutRole(int userId, IEnumerable<RoleDTO> dTOs)
		{
			var cred = await _context.Credentials
				.FindAsync(userId);
			if (cred == null) return NotFound();

			await _context.Entry(cred)
				.Collection(x => x.Roles)
				.LoadAsync();

			foreach (var dTO in dTOs)
			{
				var role = cred.Roles.FirstOrDefault(x => x.Type == dTO.Type);
				if (role == null)
				{
					role = new Role() { CredentialUserId = userId };
					cred.Roles.Add(role);
				}

				role.Expiry = dTO.Expiry;
			}

			await _context.SaveChangesAsync();

			return NoContent();
		}


		// DELETE: api/Roles/5
		[HttpDelete("{userId}")]
		public async Task<IActionResult> DeleteRole(int userId, RoleType type)
		{
			var affected = await _context.Roles
				.Where(x => x.CredentialUserId == userId && x.Type == type)
				.ExecuteDeleteAsync();

			return affected > 0 ? NoContent() : NotFound();
		}
	}
}
