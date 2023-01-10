using APICommonLibrary.Constants;
using APICommonLibrary.Validations;
using AutoMapper;
using Library.API.DTOs.Courses;
using Library.API.Infrastructure.Contexts;
using Library.API.Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
		public async Task<ActionResult<IEnumerable<CourseResult>>> GetAll(
			[FromQuery] CourseQuery query)
		{
			var results = await _context.Courses
				.AsNoTracking()
				.Where(x =>
					x.IsApproved &&
					(query.Title == null || x.Title.Contains(query.Title)) &&
					(query.TopicId == null || query.TopicId == x.TopicId) &&
					(query.PublisherUserId == null || query.PublisherUserId == x.PublisherUserId))
				.Skip(query.Page * query.PageSize)
				.Take(query.PageSize)
				.Select(x => _mapper.Map<CourseResult>(x))
				.ToListAsync();

			return Ok(results);
		}

		// GET: /courses/publisher
		[HttpGet("publisher")]
		public async Task<ActionResult<IEnumerable<CourseResult>>> GetAllByPublisher(
			[FromHeader] int publisherUserId)
		{
			var results = await _context.Courses
				.AsNoTracking()
				.Where(x => x.PublisherUserId == publisherUserId)
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
				.Where(x => !x.IsApproved)
				.Select(x => _mapper.Map<CourseResult>(x))
				.ToListAsync();

			return Ok(results);
		}

		// GET: /courses/5
		[HttpGet("{courseId}")]
		public async Task<ActionResult<CourseDetailedResult>> Get(
			[FromHeader] int? publisherUserId,
			int courseId)
		{
			CourseDetailedResult? result;
			if (publisherUserId == null)
			{
				result = await _context.Courses
					.AsNoTracking()
					.Where(x => x.CourseId == courseId && x.IsApproved)
					.Select(x => _mapper.Map<CourseDetailedResult>(x))
					.FirstOrDefaultAsync();
			}
			else
			{
				result = await _context.Courses
					.AsNoTracking()
					.Where(x => x.CourseId == courseId && x.PublisherUserId == (int)publisherUserId)
					.Select(x => _mapper.Map<CourseDetailedResult>(x))
					.FirstOrDefaultAsync();
			}
			if (result == null) return NotFound();

			return result;
		}

		// GET: /courses/5/admin
		[HttpGet("{courseId}/admin")]
		public async Task<ActionResult<CourseDetailedResult>> GetAdmin(
			int courseId)
		{
			var result = await _context.Courses
				.AsNoTracking()
				.Where(x => x.CourseId == courseId)
				.Select(x => _mapper.Map<CourseDetailedResult>(x))
				.FirstOrDefaultAsync();
			if (result == null) return NotFound();

			return result;
		}


		// POST: /courses
		[HttpPost]
		public async Task<ActionResult<CourseDetailedResult>> Create(
			CreateCourse dto)
		{
			var course = _mapper.Map<Course>(dto);

			_context.Courses.Add(course);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(Get), new { dto.PublisherUserId, course.CourseId },
				_mapper.Map<CourseDetailedResult>(course));
		}


		// PUT: /courses/5
		[HttpPut("{courseId}")]
		public async Task<ActionResult<CourseDetailedResult>> Update(
			[FromHeader] int publisherUserId,
			int courseId,
			UpdateCourse dto)
		{
			var course = await _context.Courses
				.FirstOrDefaultAsync(x => x.CourseId == courseId && x.PublisherUserId == publisherUserId);
			if (course == null) return NotFound();

			_mapper.Map(dto, course);
			await _context.SaveChangesAsync();

			var result = _mapper.Map<CourseDetailedResult>(course);

			return result;
		}


		// DELETE: /courses/5
		[HttpDelete("{courseId}")]
		public async Task<ActionResult> Delete(
			[FromHeader] int publisherUserId,
			int courseId)
		{
			var result = await _context.Courses
				.Where(x => x.CourseId == courseId && x.PublisherUserId == publisherUserId)
				.ExecuteDeleteAsync();
			if (result == 0) return NotFound();

			return NoContent();
		}


		// PUT: /courses/5/cover
		[HttpPut("{courseId}/cover")]
		public async Task<ActionResult> UpdateCover(
			[FromHeader] int publisherUserId,
			int courseId,
			[BindRequired][MaxSize(0, 2 * 1024 * 1024)][ImageExtension] IFormFile FormFile)
		{
			var course = await _context.Courses
				.FirstOrDefaultAsync(x => x.CourseId == courseId && x.PublisherUserId == publisherUserId);
			if (course == null) return NotFound();

			using var memoryStream = new MemoryStream();
			await FormFile.CopyToAsync(memoryStream);
			var extension = Path.GetExtension(FormFile.FileName).ToLowerInvariant();

			course.Cover = new CourseCover()
			{
				MediaType = FormFileContants.Extensions.GetValueOrDefault(extension)!,
				Data = memoryStream.ToArray(),
				CourseId = courseId
			};

			await _context.SaveChangesAsync();

			return NoContent();
		}
	}
}
