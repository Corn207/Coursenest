using APICommonLibrary.Constants;
using AutoMapper;
using Library.API.DTOs.Courses;
using Library.API.Infrastructure.Contexts;
using Library.API.Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class CoursesController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly DataContext _context;

		public CoursesController(IMapper mapper, DataContext context)
		{
			_mapper = mapper;
			_context = context;
		}


		// GET: /courses
		[HttpGet]
		public async Task<ActionResult<IEnumerable<CourseResult>>> GetAll([FromQuery] CourseQuery query)
		{
			var results = await _context.Courses
				.AsNoTracking()
				.Include(x => x.Cover)
				.Include(x => x.Ratings)
				.Where(x => x.IsApproved)
				.Where(x => query.Title == null || x.Title.Contains(query.Title))
				.Where(x => query.TopicId == null || query.TopicId == x.TopicId)
				.Where(x => query.PublisherUserId == null || query.PublisherUserId == x.PublisherUserId)
				.Take(query.Top ?? 5)
				.Select(x => _mapper.Map<CourseResult>(x))
				.ToListAsync();

			return Ok(results);
		}

		// GET: /courses/publisher
		[HttpGet("publisher")]
		public async Task<ActionResult<IEnumerable<CourseResult>>> GetAllByPublisher([FromHeader] int userId)
		{
			var results = await _context.Courses
				.AsNoTracking()
				.Include(x => x.Cover)
				.Include(x => x.Ratings)
				.Where(x => x.PublisherUserId == userId)
				.Select(x => _mapper.Map<CourseResult>(x))
				.ToListAsync();

			return Ok(results);
		}

		// GET: /courses/unapproved (Admin only)
		[HttpGet("unapproved")]
		public async Task<ActionResult<IEnumerable<CourseResult>>> GetAllUnapproved()
		{
			var results = await _context.Courses
				.AsNoTracking()
				.Include(x => x.Cover)
				.Include(x => x.Ratings)
				.Where(x => !x.IsApproved)
				.Select(x => _mapper.Map<CourseResult>(x))
				.ToListAsync();

			return Ok(results);
		}

		// GET: /courses/5
		[HttpGet("{courseId}")]
		public async Task<ActionResult<CourseDetailedResult>> Get(int courseId)
		{
			var result = await _context.Courses
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.IsApproved && x.CourseId == courseId);
			if (result == null) return NotFound();

			return _mapper.Map<CourseDetailedResult>(result);
		}

		// GET: /courses/5/publisher
		[HttpGet("{courseId}/publisher")]
		public async Task<ActionResult<CourseDetailedResult>> GetByPublisher(int courseId, [FromHeader] int userId)
		{
			var result = await _context.Courses
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.CourseId == courseId && x.PublisherUserId == userId);
			if (result == null) return NotFound();

			return _mapper.Map<CourseDetailedResult>(result);
		}

		// GET: /courses/5/admin
		[HttpGet("{courseId}/admin")]
		public async Task<ActionResult<CourseDetailedResult>> GetAdmin(int courseId)
		{
			var result = await _context.Courses
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.CourseId == courseId);
			if (result == null) return NotFound();

			return _mapper.Map<CourseDetailedResult>(result);
		}


		// POST: /courses
		[HttpPost]
		public async Task<ActionResult<CourseResult>> Create(CreateCourse dto)
		{
			var course = _mapper.Map<Course>(dto);

			_context.Courses.Add(course);

			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetByPublisher), new { course.CourseId }, course);
		}


		// PUT: /courses/5
		[HttpPut("{courseId}")]
		public async Task<ActionResult> Update(int courseId, UpdateCourse dto)
		{
			var result = await _context.Courses
				.FirstOrDefaultAsync(x => x.CourseId == courseId);
			if (result == null) return NotFound();
			if (result.PublisherUserId != dto.PublisherUserId) return Forbid();

			_mapper.Map(dto, result);

			await _context.SaveChangesAsync();

			return NoContent();
		}


		// DELETE: /courses/5
		[HttpDelete("{courseId}")]
		public async Task<ActionResult> Delete(int courseId, [FromHeader] int userId)
		{
			var result = await _context.Courses
				.FirstOrDefaultAsync(x => x.CourseId == courseId);
			if (result == null) return NotFound();
			if (result.PublisherUserId != userId) return Forbid();

			_context.Courses.Remove(result);

			await _context.SaveChangesAsync();

			return NoContent();
		}


		// PUT: /courses/5/cover
		[HttpPut("{courseId}/cover")]
		public async Task<ActionResult> UpdateCover(UpdateCourseCover dto)
		{
			var result = await _context.Courses.FindAsync(dto.CourseId);
			if (result == null) return NotFound();
			if (result.PublisherUserId != dto.UserId) return Forbid();

			using var memoryStream = new MemoryStream();
			await dto.FormFile.CopyToAsync(memoryStream);

			result.LastModified = DateTime.Now;
			var extension = Path.GetExtension(dto.FormFile.FileName).ToLowerInvariant();
			var cover = new CourseCover()
			{
				MediaType = FormFileContants.Extensions.GetValueOrDefault(extension)!,
				Data = memoryStream.ToArray(),
				CourseId = dto.CourseId
			};

			_context.CourseCovers.Update(cover);

			await _context.SaveChangesAsync();

			return NoContent();
		}


		// GET: /courses/5/ratings
		[HttpGet("{courseId}/ratings")]
		public async Task<ActionResult<IEnumerable<RatingResult>>> GetAllRating(int courseId)
		{
			var exist = await _context.Courses
				.AsNoTracking()
				.AnyAsync(x => x.CourseId == courseId);
			if (!exist) return NotFound();

			var results = await _context.Ratings
				.Where(x => x.CourseId == courseId)
				.Select(x => _mapper.Map<RatingResult>(x))
				.ToListAsync();

			return results;
		}

		// GET: /courses/5/ratings/me
		[HttpGet("{courseId}/ratings/me")]
		public async Task<ActionResult<RatingResult>> GetRating(int courseId, [FromHeader] int userId)
		{
			var result = await _context.Ratings
				.AsNoTracking()
				.Where(x => x.CourseId == courseId && x.UserId == userId)
				.Select(x => _mapper.Map<RatingResult>(x))
				.FirstOrDefaultAsync();
			if (result == null) return NotFound();

			return result;
		}

		// GET: /courses/5/ratings/stat
		[HttpGet("{courseId}/ratings/stat")]
		public async Task<ActionResult<RatingStatResult>> GetRatingStat(int courseId)
		{
			var averageStars = _context.Ratings
				.AsNoTracking()
				.Where(x => x.CourseId == courseId)
				.AverageAsync(x => x.Stars);
			var total = _context.Ratings
				.CountAsync(x => x.CourseId == courseId);

			await Task.WhenAll(averageStars, total);

			return new RatingStatResult()
			{
				AverageStars = averageStars.Result,
				Total = total.Result
			};
		}

		// POST: /courses/5/ratings
		[HttpPost("{courseId}/ratings")]
		public async Task<ActionResult<RatingResult>> CreateRating(CreateRating dto)
		{
			var result = _mapper.Map<Rating>(dto);

			_context.Ratings.Add(result);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException)
			{
				return Conflict();
			}

			return _mapper.Map<RatingResult>(result);
		}

		// DELETE: /courses/5/ratings/me
		[HttpDelete("{courseId}/ratings/me")]
		public async Task<ActionResult> DeleteRating(int courseId, [FromHeader] int userId)
		{
			var result = await _context.Ratings
				.Where(x => x.CourseId == courseId && x.UserId == userId)
				.ExecuteDeleteAsync();
			if (result == 0) return NotFound();

			return NoContent();
		}
	}
}
