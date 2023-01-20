using APICommonLibrary.Constants;
using APICommonLibrary.MessageBus.Commands;
using APICommonLibrary.MessageBus.Events;
using APICommonLibrary.MessageBus.Responses;
using APICommonLibrary.Models;
using APICommonLibrary.Utilities.APIs;
using APICommonLibrary.Validations;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Identity.API.DTOs;
using Identity.API.Infrastructure.Contexts;
using Identity.API.Infrastructure.Entities;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
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
		private readonly IRequestClient<CheckTopicIds> _checkTopicIdsClient;
		private readonly IPublishEndpoint _publishEndpoint;

		public UsersController(
			IMapper mapper,
			DataContext context,
			IRequestClient<CheckTopicIds> checkTopicsClient,
			IPublishEndpoint publishEndpoint)
		{
			_mapper = mapper;
			_context = context;
			_checkTopicIdsClient = checkTopicsClient;
			_publishEndpoint = publishEndpoint;
		}


		// GET: /users
		[HttpGet]
		[Authorize(Roles = nameof(RoleTypes.Admin))]
		public async Task<ActionResult<IEnumerable<UserResult>>> GetAll(
			[FromQuery] UserQuery query)
		{
			var results = await _context.Users
				.Where(x =>
					string.IsNullOrWhiteSpace(query.FullName) ||
					x.FullName.Contains(query.FullName))
				.Skip(query.Page * query.PageSize)
				.Take(query.PageSize)
				.ProjectTo<UserResult>(_mapper.ConfigurationProvider)
				.ToListAsync();

			return results;
		}

		// GET: /users/count
		[HttpGet("count")]
		[Authorize(Roles = nameof(RoleTypes.Admin))]
		public async Task<ActionResult<int>> GetCount(
			[FromQuery] string? fullName)
		{
			var result = await _context.Users
				.Where(x =>
					string.IsNullOrWhiteSpace(fullName) ||
					x.FullName.Contains(fullName))
				.CountAsync();

			return result;
		}

		// GET: /users/5
		[HttpGet("{userId}")]
		[AllowAnonymous]
		public async Task<ActionResult<UserResult>> Get(
			int userId)
		{
			var result = await _context.Users
				.ProjectTo<UserResult>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(x => x.UserId == userId);
			if (result == null)
				return NotFound($"UserId: {userId} does not exist.");

			return result;
		}

		// GET: /users/5/profile
		[HttpGet("{userId}/profile")]
		[AllowAnonymous]
		public async Task<ActionResult<UserProfileResult>> GetProfile(
			int userId)
		{
			var result = await _context.Users
				.AsNoTracking()
				.ProjectTo<UserProfileResult>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(x => x.UserId == userId);
			if (result == null)
				return NotFound($"UserId: {userId} does not exist.");

			return result;
		}

		// GET: /users/5/instructor
		[HttpGet("{userId}/instructor")]
		[AllowAnonymous]
		public async Task<ActionResult<UserInstructorResult>> GetInstructor(
			int userId)
		{
			var result = await _context.Users
				.ProjectTo<UserInstructorResult>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(x => x.UserId == userId);
			if (result == null)
				return NotFound($"UserId: {userId} does not exist.");

			return result;
		}


		// PUT: /users/me
		[HttpPut("me")]
		[Authorize]
		public async Task<ActionResult> Update(
			[FromBody] UpdateUser body)
		{
			var userId = GetUserId();

			var user = await _context.Users
				.FirstOrDefaultAsync(x => x.UserId == userId);
			if (user == null)
				return NotFound($"UserId: {userId} does not exist.");

			var emailExists = await _context.Users
				.AnyAsync(x => x.Email == body.Email);
			if (emailExists)
				return Conflict("Email existed.");

			_mapper.Map(body, user);

			await _context.SaveChangesAsync();

			return NoContent();
		}


		// DELETE: /users/5
		[HttpDelete("{userId}")]
		[Authorize(Roles = nameof(RoleTypes.Admin))]
		public async Task<ActionResult> Delete(
			int userId)
		{
			var affected = await _context.Users
				.Where(x => x.UserId == userId)
				.ExecuteDeleteAsync();
			if (affected == 0)
				return NotFound($"UserId: {userId} does not exist.");

			var request = new UserDeleted() { UserId = userId };
			await _publishEndpoint.Publish(request);

			return NoContent();
		}


		// PUT: /users/me/cover
		[HttpPut("me/cover")]
		[Authorize]
		public async Task<ActionResult> UpdateCover(
			[BindRequired][MaxSize(0, 2 * 1024 * 1024)][ImageExtension] IFormFile formFile)
		{
			var userId = GetUserId();

			var user = await _context.Users
				.Include(x => x.Avatar)
				.FirstOrDefaultAsync(x => x.UserId == userId);
			if (user == null)
				return NotFound($"UserId: {userId} does not exist.");

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


		// POST: /users/me/interest
		[HttpPost("me/interest")]
		[Authorize]
		public async Task<ActionResult<int>> AddInterestedTopic(
			[FromBody] int topicId)
		{
			var userId = GetUserId();

			var user = await _context.Users
				.Select(x =>
					new
					{
						x.UserId,
						InterestedTopicIds = x.InterestedTopics.Select(x => x.TopicId)
					})
				.FirstOrDefaultAsync(x => x.UserId == userId);
			if (user == null)
				return NotFound($"UserId: {userId} does not exist.");
			if (user.InterestedTopicIds.Contains(topicId))
				return Conflict($"InterestedTopicId: {topicId} existed.");

			var request = new CheckTopicIds()
			{
				TopicIds = new[] { topicId }
			};
			var response = await _checkTopicIdsClient
				.GetResponse<Existed, NotFound>(request);

			if (response.Is(out Response<NotFound>? notFoundResponse))
			{
				return NotFound(notFoundResponse!.Message.Message);
			}

			var topic = new InterestedTopic()
			{
				UserId = userId,
				TopicId = topicId
			};
			_context.InterestedTopics.Add(topic);

			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetProfile), new { userId });
		}

		// DELETE: /users/me/interest/5
		[HttpDelete("me/interest/{topicId}")]
		[Authorize]
		public async Task<ActionResult> DeleteInterestedTopic(
			int topicId)
		{
			var userId = GetUserId();

			var result = await _context.InterestedTopics
				.Where(x => x.UserId == userId && x.TopicId == topicId)
				.ExecuteDeleteAsync();
			if (result == 0)
				return NotFound($"InterestedTopicId: {topicId} does not exists.");

			return NoContent();
		}


		// GET: /users/me/follow
		[HttpGet("me/follow")]
		[Authorize]
		public async Task<ActionResult<IEnumerable<int>>> GetAllFollowedTopic()
		{
			var userId = GetUserId();

			var results = await _context.Users
				.Where(x => x.UserId == userId)
				.Select(x => x.FollowedTopics.Select(x => x.TopicId))
				.FirstOrDefaultAsync();
			if (results == null)
				return NotFound($"UserId: {userId} does not exist.");

			return Ok(results);
		}


		// POST: /users/me/follow
		[HttpPost("me/follow")]
		[Authorize]
		public async Task<ActionResult> AddFollowedTopic(
			[FromBody] int topicId)
		{
			var userId = GetUserId();

			var user = await _context.Users
				.Select(x =>
					new
					{
						x.UserId,
						FollowedTopicIds = x.FollowedTopics.Select(x => x.TopicId)
					})
				.FirstOrDefaultAsync(x => x.UserId == userId);
			if (user == null)
				return NotFound($"UserId: {userId} does not exist.");
			if (user.FollowedTopicIds.Contains(topicId))
				return Conflict($"FollowedTopicId: {topicId} existed.");

			var request = new CheckTopicIds()
			{
				TopicIds = new[] { topicId }
			};
			var response = await _checkTopicIdsClient
				.GetResponse<Existed, NotFound>(request);

			if (response.Is(out Response<NotFound>? notFoundResponse))
			{
				return NotFound(notFoundResponse!.Message.Message);
			}

			var topic = new FollowedTopic()
			{
				UserId = userId,
				TopicId = topicId
			};

			_context.FollowedTopics.Add(topic);

			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetProfile), new { userId });
		}


		// DELETE: /users/me/follow/5
		[HttpDelete("me/follow/{topicId}")]
		public async Task<ActionResult> DeleteFollowedTopic(
			int topicId)
		{
			var userId = GetUserId();

			var result = await _context.FollowedTopics
				.Where(x => x.UserId == userId && x.TopicId == topicId)
				.ExecuteDeleteAsync();
			if (result == 0)
				return NotFound($"FollowedTopicId: {topicId} does not exists.");

			return NoContent();
		}


		// POST: /users/me/experiences
		[HttpPost("me/experiences")]
		public async Task<ActionResult> AddExperience(
			[FromBody] CreateExperience body)
		{
			var userId = GetUserId();

			var user = await _context.Users
				.Include(x => x.Experiences)
				.FirstOrDefaultAsync(x => x.UserId == userId);
			if (user == null)
				return NotFound($"UserId: {userId} does not exist.");

			var experience = _mapper.Map<Experience>(body);
			experience.UserId = userId;

			user.Experiences.Add(experience);
			user.LastModified = DateTime.Now;

			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetProfile), new { userId });
		}


		// DELETE: /users/me/experiences/2
		[HttpDelete("me/experiences/{experienceId}")]
		public async Task<ActionResult> DeleteExperience(
			int experienceId)
		{
			var userId = GetUserId();

			var affected = await _context.Experiences
				.Where(x => x.UserId == userId && x.ExperienceId == experienceId)
				.ExecuteDeleteAsync();
			if (affected == 0)
				return NotFound($"ExperienceId: {experienceId} does not exist.");

			return NoContent();
		}


		public int GetUserId()
		{
			return ClaimUtilities.GetUserId(User.Claims);
		}
	}
}
