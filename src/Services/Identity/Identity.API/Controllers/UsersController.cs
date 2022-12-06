using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Identity.API.Contexts;
using Identity.API.Models;
using Identity.API.DTOs;
using AutoMapper;

namespace Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UsersController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers()
        {
            return await _context.Users
                .Select(m => _mapper.Map<UserResponse>(m))
                .ToListAsync();
        }

        // GET: api/Users/5/profile
        [HttpGet("{id}/profile")]
        public async Task<ActionResult<ProfileResponse>> GetUserProfile(int id)
        {
            var user = await _context.Users
                .Include(x => x.AvatarImage)
                .Include(x => x.Experiences)
                .Include(x => x.InterestedTopics)
                .FirstOrDefaultAsync(x => x.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return _mapper.Map<ProfileResponse>(user);
        }

        // GET: api/Users/5/info
        [HttpGet("{id}/info")]
        public async Task<ActionResult<InfoResponse>> GetUserInfo(int id)
        {
            var user = await _context.Users
                .Include(x => x.AvatarImage)
                .FirstOrDefaultAsync(x => x.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return _mapper.Map<InfoResponse>(user);
        }

        // GET: api/Users/5/instructor
        [HttpGet("{id}/instructor")]
        public async Task<ActionResult<InstructorResponse>> GetUserInstructor(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return _mapper.Map<InstructorResponse>(user);
        }


        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, EditUserRequest userRequest)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            _mapper.Map(userRequest, user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }


        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(AddUserRequest request)
        {
            var user = _mapper.Map<User>(request);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserInfo", new { id = user.UserId }, user);
        }


        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
