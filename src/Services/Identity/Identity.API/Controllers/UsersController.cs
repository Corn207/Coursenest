using APICommonLibrary.Constants;
using APICommonLibrary.MessageBus.Commands;
using AutoMapper;
using Identity.API.DTOs;
using Identity.API.Infrastructure.Contexts;
using Identity.API.Infrastructure.Entities;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly DataContext _context;
		private readonly IRequestClient<GetTopic> _getTopicClient;

		public UsersController(
			IMapper mapper,
			DataContext context,
			IRequestClient<GetTopic> getTopicClient)
		{
			_mapper = mapper;
			_context = context;
			_getTopicClient = getTopicClient;
		}


		// GET: /users
		[HttpGet]
		public async Task<ActionResult<IEnumerable<UserResult>>> GetAll()
		{
			var results = await _context.Users
				.AsNoTracking()
				.Include(x => x.Avatar)
				.Select(x => _mapper.Map<UserResult>(x))
				.ToListAsync();

			return results;
		}

		// GET: /users/5
		[HttpGet("{userId}")]
		public async Task<ActionResult<UserResult>> Get(int userId)
		{
			var result = await _context.Users
				.AsNoTracking()
				.Include(x => x.Avatar)
				.Select(x => _mapper.Map<UserResult>(x))
				.FirstOrDefaultAsync(x => x.UserId == userId);
			if (result == null) return NotFound();

			return result;
		}

		// GET: /users/5/profile
		[HttpGet("{userId}/profile")]
		public async Task<ActionResult<UserProfileResult>> GetProfile(int userId)
		{
			var exist = await _context.Users
				.AsNoTracking()
				.AnyAsync(x => x.UserId == userId);
			if (!exist) return NotFound();

			var result = await _context.Users
				.AsNoTracking()
				.Include(x => x.Avatar)
				.Include(x => x.Experiences)
				.Include(x => x.InterestedTopics)
				.Select(x => _mapper.Map<UserProfileResult>(x))
				.FirstOrDefaultAsync(x => x.UserId == userId);
			if (result == null) return NotFound();

			return result;
		}

		// GET: /users/5/instructor
		[HttpGet("{userId}/instructor")]
		public async Task<ActionResult<UserInstructorResult>> GetInstructor(int userId)
		{
			var result = await _context.Users
				.AsNoTracking()
				.Include(x => x.Avatar)
				.Select(x => _mapper.Map<UserInstructorResult>(x))
				.FirstOrDefaultAsync(x => x.UserId == userId);
			if (result == null) return NotFound();

			return result;
		}


		// PUT: /users/me
		[HttpPut("me")]
		public async Task<ActionResult> Update([FromHeader] int userId, UpdateUser dto)
		{
			var result = await _context.Users
				.FirstOrDefaultAsync(x => x.UserId == userId);
			if (result == null) return NotFound();

			_mapper.Map(dto, result);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException)
			{
				return Conflict("Email existed.");
			}

			return NoContent();
		}


		// DELETE: /users/5
		[HttpDelete("{userId}")]
		public async Task<ActionResult> Delete(int userId)
		{
			var result = await _context.Users
				.Where(x => x.UserId == userId)
				.ExecuteDeleteAsync();
			if (result == 0) return NotFound();

			return NoContent();
		}


		// PUT: /users/me/cover
		[HttpPut("me/cover")]
		public async Task<ActionResult> UpdateCover(UpdateAvatar dto)
		{
			var result = await _context.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.UserId == dto.UserId);
			if (result == null) return NotFound();

			using var memoryStream = new MemoryStream();
			await dto.FormFile.CopyToAsync(memoryStream);

			var extension = Path.GetExtension(dto.FormFile.FileName).ToLowerInvariant();
			var avatar = new Avatar()
			{
				MediaType = FormFileContants.Extensions.GetValueOrDefault(extension)!,
				Data = memoryStream.ToArray(),
				UserId = dto.UserId,
			};

			result.LastModified = DateTime.Now;
			_context.Avatars.Update(avatar);

			//await _context.Avatars.Where(x => x.UserId == dto.UserId).ExecuteDeleteAsync();

			//var extension = Path.GetExtension(dto.FormFile.FileName).ToLowerInvariant();
			//result.Avatar = new Avatar()
			//{
			//	MediaType = FormFileContants.Extensions.GetValueOrDefault(extension)!,
			//	Data = memoryStream.ToArray()
			//};

			await _context.SaveChangesAsync();

			return NoContent();
		}


		// POST: /users/me/interested
		[HttpPost("me/interested")]
		public async Task<ActionResult<int>> AddInterestedTopic([FromHeader] int userId, int topicId)
		{
			var result = await _context.Users.FindAsync(userId);
			if (result == null) return NotFound();

			var getTopic = new GetTopic() { TopicId = topicId };
			Response<GetTopicResult> getTopicResponse;
			try
			{
				getTopicResponse = await _getTopicClient.GetResponse<GetTopicResult>(getTopic);
			}
			catch (KeyNotFoundException)
			{
				return NotFound("TopicId not existed.");
			}

			result.InterestedTopics.Add(new InterestedTopic() { TopicId = topicId });

			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetProfile), userId, topicId);
		}


		// DELETE: /users/me/interested
		[HttpDelete("me/interested")]
		public async Task<ActionResult> DeleteInterestedTopic([FromHeader] int userId, int topicId)
		{
			var exist = await _context.Users
				.AsNoTracking()
				.AnyAsync(x => x.UserId == userId);
			if (!exist) return NotFound();

			var result = await _context.InterestedTopics
				.Where(x => x.UserId == userId && x.TopicId == topicId)
				.ExecuteDeleteAsync();
			if (result == 0) return NotFound("TopicId not interested.");

			return NoContent();
		}


		// POST: /users/me/followed
		[HttpPost("me/followed")]
		public async Task<ActionResult<int>> AddFollowedTopic([FromHeader] int userId, int topicId)
		{
			var result = await _context.Users.FindAsync(userId);
			if (result == null) return NotFound();

			var getTopic = new GetTopic() { TopicId = topicId };
			Response<GetTopicResult> getTopicResponse;
			try
			{
				getTopicResponse = await _getTopicClient.GetResponse<GetTopicResult>(getTopic);
			}
			catch (KeyNotFoundException)
			{
				return NotFound("TopicId not existed.");
			}

			result.FollowedTopics.Add(new FollowedTopic() { TopicId = topicId });

			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetProfile), userId, topicId);
		}


		// DELETE: /users/me/followed
		[HttpDelete("me/followed")]
		public async Task<ActionResult> DeleteFollowedTopic([FromHeader] int userId, int topicId)
		{
			var exist = await _context.Users
				.AsNoTracking()
				.AnyAsync(x => x.UserId == userId);
			if (!exist) return NotFound();

			var result = await _context.FollowedTopics
				.Where(x => x.UserId == userId && x.TopicId == topicId)
				.ExecuteDeleteAsync();
			if (result == 0) return NotFound("TopicId not followed.");

			return NoContent();
		}


		// POST: /users/me/experiences
		[HttpPost("me/experiences")]
		public async Task<ActionResult<ExperienceResult>> AddExperience([FromHeader] int userId, CreateExperience dto)
		{
			var user = await _context.Users.FindAsync(userId);
			if (user == null) return NotFound();

			var result = _mapper.Map<Experience>(dto);

			user.Experiences.Add(result);
			user.LastModified = DateTime.Now;

			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetProfile), userId, _mapper.Map<ExperienceResult>(result));
		}


		// DELETE: /users/me/experiences
		[HttpDelete("me/experiences")]
		public async Task<ActionResult> DeleteExperience([FromHeader] int userId, int experienceId)
		{
			var exist = await _context.Users
				.AsNoTracking()
				.AnyAsync(x => x.UserId == userId);
			if (!exist) return NotFound();

			var result = await _context.Experience
				.Where(x => x.UserId == userId && x.ExperienceId == experienceId)
				.ExecuteDeleteAsync();
			if (result == 0) return NotFound("ExperienceId not existed.");

			return NoContent();
		}
	}
}
