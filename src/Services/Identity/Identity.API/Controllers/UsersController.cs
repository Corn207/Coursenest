using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Identity.API.Contexts;
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
                .Select(x => _mapper.Map<UserResponse>(x))
                .ToListAsync();
        }

        // GET: api/Users/5/Profile
        [HttpGet("{userId}/Profile")]
        public async Task<ActionResult<UserProfileResponse>> GetUserProfile(int userId)
        {
            var user = await _context.Users
                .Include(x => x.Avatar)
                .Include(x => x.Experiences)
                .Include(x => x.InterestedTopics)
                .Include(x => x.FollowedTopics)
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (user == null) return NotFound();

            return _mapper.Map<UserProfileResponse>(user);
        }

        // GET: api/Users/5/Info
        [HttpGet("{userId}/Info")]
        public async Task<ActionResult<UserInfoResponse>> GetUserInfo(int userId)
        {
            var user = await _context.Users
                .Include(x => x.Avatar)
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (user == null) return NotFound();

            return _mapper.Map<UserInfoResponse>(user);
        }

        // GET: api/Users/5/Instructor
        [HttpGet("{userId}/Instructor")]
        public async Task<ActionResult<UserInstructorResponse>> GetUserInstructor(int userId)
        {
            var user = await _context.Users
                .Include(x => x.Avatar)
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (user == null) return NotFound();

            return _mapper.Map<UserInstructorResponse>(user);
        }


        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserPutRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserId == id);
            if (user == null) return NotFound();

            _mapper.Map(request, user);
            user.LastModified = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            catch (DbUpdateException)
            {
                return Conflict(new { message = $"Existed email ({request.Email}) with userId ({user.UserId})." });
            }
            
            return NoContent();
        }


        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            // TODO Delete user -> Delete other microservice refs

            return NoContent();
        }
    }
}
