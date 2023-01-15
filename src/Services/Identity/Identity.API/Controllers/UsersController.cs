using APICommonLibrary.Constants;
using APICommonLibrary.MessageBus.Commands;
using APICommonLibrary.MessageBus.Events;
using APICommonLibrary.MessageBus.Responses;
using APICommonLibrary.Validations;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Identity.API.DTOs;
using Identity.API.Infrastructure.Contexts;
using Identity.API.Infrastructure.Entities;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly DataContext _context;
		private readonly IRequestClient<CheckTopics> _checkTopicsClient;
		private readonly IPublishEndpoint _publishEndpoint;

		public UsersController(
			IMapper mapper,
			DataContext context,
			IRequestClient<CheckTopics> checkTopicsClient,
			IPublishEndpoint publishEndpoint)
		{
			_mapper = mapper;
			_context = context;
			_checkTopicsClient = checkTopicsClient;
			_publishEndpoint = publishEndpoint;
		}


		// GET: /users
		[HttpGet]
		public async Task<ActionResult<IEnumerable<UserResult>>> GetAll(
			[FromQuery] UserQuery query)
		{
			var results = await _context.Users
				.AsNoTracking()
				.Where(x => string.IsNullOrWhiteSpace(query.FullName) || x.FullName.Contains(query.FullName))
				.Skip(query.Page * query.PageSize)
				.Take(query.PageSize)
				.ProjectTo<UserResult>(_mapper.ConfigurationProvider)
				.ToListAsync();

			return results;
		}

		// GET: /users/count
		[HttpGet("count")]
		public async Task<ActionResult<int>> GetCount()
		{
			var result = await _context.Users
				.AsNoTracking()
				.CountAsync();

			return result;
		}

		// GET: /users/5
		[HttpGet("{userId}")]
		public async Task<ActionResult<UserResult>> Get(
			int userId)
		{
			var result = await _context.Users
				.AsNoTracking()
				.ProjectTo<UserResult>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(x => x.UserId == userId);
			if (result == null) return NotFound();

			return result;
		}

		// GET: /users/5/profile
		[HttpGet("{userId}/profile")]
		public async Task<ActionResult<UserProfileResult>> GetProfile(
			int userId)
		{
			var result = await _context.Users
				.AsNoTracking()
				.ProjectTo<UserProfileResult>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(x => x.UserId == userId);
			if (result == null) return NotFound();

			return result;
		}

		// GET: /users/5/instructor
		[HttpGet("{userId}/instructor")]
		public async Task<ActionResult<UserInstructorResult>> GetInstructor(
			int userId)
		{
			var result = await _context.Users
				.AsNoTracking()
				.ProjectTo<UserInstructorResult>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(x => x.UserId == userId);
			if (result == null) return NotFound();

			return result;
		}


		// PUT: /users/me
		[HttpPut("me")]
		public async Task<ActionResult> Update(
			[FromHeader] int userId,
			UpdateUser dto)
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
		public async Task<ActionResult> Delete(
			int userId)
		{
			var result = await _context.Users
				.Where(x => x.UserId == userId)
				.ExecuteDeleteAsync();
			if (result == 0) return NotFound();

			var publish = new UserDeleted() { UserId = userId };
			await _publishEndpoint.Publish(publish);

			return NoContent();
		}


		// PUT: /users/me/cover
		[HttpPut("me/cover")]
		public async Task<ActionResult> UpdateCover(
			[FromHeader] int userId,
			[BindRequired][MaxSize(0, 2 * 1024 * 1024)][ImageExtension] IFormFile FormFile)
		{
			var user = await _context.Users
				.Include(x => x.Avatar)
				.FirstOrDefaultAsync(x => x.UserId == userId);
			if (user == null) return NotFound();

			using var memoryStream = new MemoryStream();
			await FormFile.CopyToAsync(memoryStream);
			var extension = Path.GetExtension(FormFile.FileName).ToLowerInvariant();

			user.Avatar = new Avatar()
			{
				MediaType = FormFileContants.Extensions.GetValueOrDefault(extension)!,
				Data = memoryStream.ToArray(),
				UserId = userId,
			};
			user.LastModified = DateTime.Now;

			await _context.SaveChangesAsync();

			return NoContent();
		}


		// POST: /users/me/interest/2
		[HttpPost("me/interest/{topicId}")]
		public async Task<ActionResult<int>> AddInterestedTopic(
			[FromHeader] int userId,
			int topicId)
		{
			var exists = await _context.Users
				.AsNoTracking()
				.AnyAsync(x => x.UserId == userId);
			if (!exists) return NotFound();

			var checkTopicsRequest = new CheckTopics() { TopicIds = new[] { topicId } };
			var checkTopicsResponse = await _checkTopicsClient.GetResponse<Existed, NotFound>(checkTopicsRequest);

			if (checkTopicsResponse.Is(out Response<NotFound>? notFoundResponse))
			{
				return NotFound(notFoundResponse!.Message.Message);
			}

			var topic = new InterestedTopic() { UserId = userId, TopicId = topicId };

			_context.InterestedTopics.Add(topic);
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (Exception)
			{
				return Conflict("InterestedTopic existed.");
			}

			return CreatedAtAction(nameof(GetProfile), new { userId }, topicId);
		}

		// DELETE: /users/me/interest/5
		[HttpDelete("me/interest/{topicId}")]
		public async Task<ActionResult> DeleteInterestedTopic(
			[FromHeader] int userId,
			int topicId)
		{
			var result = await _context.InterestedTopics
				.Where(x => x.UserId == userId && x.TopicId == topicId)
				.ExecuteDeleteAsync();
			if (result == 0) return NotFound("TopicId not interested.");

			return NoContent();
		}


		// GET: /users/me/follow
		[HttpGet("me/follow")]
		public async Task<ActionResult<IEnumerable<int>>> GetAllFollowedTopic(
			[FromHeader] int userId)
		{
			var result = await _context.Users
				.AsNoTracking()
				.Include(x => x.FollowedTopics)
				.Where(x => x.UserId == userId)
				.Select(x => x.FollowedTopics.Select(x => x.TopicId))
				.FirstOrDefaultAsync();
			if (result == null) return NotFound();

			return Ok(result);
		}


		// POST: /users/me/follow/2
		[HttpPost("me/follow/{topicId}")]
		public async Task<ActionResult<int>> AddFollowedTopic(
			[FromHeader] int userId,
			int topicId)
		{
			var exists = await _context.Users
				.AsNoTracking()
				.AnyAsync(x => x.UserId == userId);
			if (!exists) return NotFound();

			var checkTopicsRequest = new CheckTopics() { TopicIds = new[] { topicId } };
			var checkTopicsResponse = await _checkTopicsClient.GetResponse<Existed, NotFound>(checkTopicsRequest);

			if (checkTopicsResponse.Is(out Response<NotFound>? notFoundResponse))
			{
				return NotFound(notFoundResponse!.Message.Message);
			}

			var topic = new FollowedTopic() { UserId = userId, TopicId = topicId };

			_context.FollowedTopics.Add(topic);
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (Exception)
			{
				return Conflict("FollowedTopic existed.");
			}

			return CreatedAtAction(nameof(GetProfile), new { userId }, topicId);
		}


		// DELETE: /users/me/follow/2
		[HttpDelete("me/follow/{topicId}")]
		public async Task<ActionResult> DeleteFollowedTopic(
			[FromHeader] int userId,
			int topicId)
		{
			var result = await _context.FollowedTopics
				.Where(x => x.UserId == userId && x.TopicId == topicId)
				.ExecuteDeleteAsync();
			if (result == 0) return NotFound("TopicId not followed.");

			return NoContent();
		}


		// POST: /users/me/experiences
		[HttpPost("me/experiences")]
		public async Task<ActionResult<ExperienceResult>> AddExperience(
			[FromHeader] int userId,
			CreateExperience dto)
		{
			var user = await _context.Users.FindAsync(userId);
			if (user == null) return NotFound();

			var experience = _mapper.Map<Experience>(dto);
			experience.UserId = userId;

			_context.Experiences.Add(experience);
			user.LastModified = DateTime.Now;

			await _context.SaveChangesAsync();

			var result = _mapper.Map<ExperienceResult>(experience);

			return CreatedAtAction(nameof(GetProfile), new { userId }, result);
		}


		// DELETE: /users/me/experiences/2
		[HttpDelete("me/experiences/{experienceId}")]
		public async Task<ActionResult> DeleteExperience(
			[FromHeader] int userId,
			int experienceId)
		{
			var result = await _context.Experiences
				.Where(x => x.UserId == userId && x.ExperienceId == experienceId)
				.ExecuteDeleteAsync();
			if (result == 0) return NotFound("ExperienceId not existed.");

			return NoContent();
		}
	}
}
