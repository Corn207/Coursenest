using AutoMapper;
using Library.API.DTOs;
using Library.API.DTOs.Lessons;
using Library.API.DTOs.Units;
using Library.API.Infrastructure.Contexts;
using Library.API.Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class LessonsController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly DataContext _context;

		public LessonsController(IMapper mapper, DataContext context)
		{
			_mapper = mapper;
			_context = context;
		}


		private async Task<bool> IsOwned(int courseId, int publisherUserId)
		{
			var result = await _context.Courses
				.AsNoTracking()
				.AnyAsync(x => x.CourseId == courseId && x.PublisherUserId == publisherUserId);

			return result;
		}

		// GET: /lessons
		[HttpGet()]
		public async Task<ActionResult<IEnumerable<LessonResult>>> GetAll(
			[FromHeader] int? publisherUserId,
			int courseId)
		{
			IEnumerable<LessonResult> results;
			if (publisherUserId == null)
			{
				results = await _context.Lessons
					.AsNoTracking()
					.Where(x => x.CourseId == courseId && x.Course.IsApproved)
					.Select(x => _mapper.Map<LessonResult>(x))
					.ToListAsync();
			}
			else
			{
				if (await IsOwned(courseId, (int)publisherUserId)) return NotFound();

				results = await _context.Lessons
					.AsNoTracking()
					.Where(x => x.CourseId == courseId)
					.Select(x => _mapper.Map<LessonResult>(x))
					.ToListAsync();
			}

			return Ok(results);
		}

		// GET: /lessons/5
		[HttpGet("{lessonId}")]
		public async Task<ActionResult<LessonResult>> Get(
			[FromHeader] int? publisherUserId,
			int lessonId)
		{
			LessonResult? result;
			if (publisherUserId == null)
			{
				result = await _context.Lessons
					.AsNoTracking()
					.Where(x => x.LessonId == lessonId && x.Course.IsApproved)
					.Select(x => _mapper.Map<LessonResult>(x))
					.FirstOrDefaultAsync();
			}
			else
			{
				result = await _context.Lessons
					.AsNoTracking()
					.Where(x => x.LessonId == lessonId && x.Course.PublisherUserId == (int)publisherUserId)
					.Select(x => _mapper.Map<LessonResult>(x))
					.FirstOrDefaultAsync();
			}
			if (result == null) return NotFound();

			return result;
		}


		// POST: /lessons
		[HttpPost]
		public async Task<ActionResult<LessonResult>> Create(
			[FromHeader] int publisherUserId,
			CreateLesson dto)
		{
			if (await IsOwned(dto.CourseId, publisherUserId))
				return NotFound();

			var result = _mapper.Map<Lesson>(dto);

			var max = await _context.Lessons
				.Where(x => x.CourseId == dto.CourseId)
				.OrderByDescending(x => x.Order)
				.Select(x => x.Order)
				.FirstOrDefaultAsync();
			result.OrderNumerator = max == default ? 1 : (int)Math.Ceiling(max);
			result.OrderDenominator = 1;

			_context.Lessons.Add(result);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(Get), new { publisherUserId, result.LessonId }, _mapper.Map<LessonResult>(result));
		}


		// PUT: /lessons/5
		[HttpPut("{lessonId}")]
		public async Task<ActionResult<LessonResult>> Update(
			[FromHeader] int publisherUserId,
			int lessonId,
			UpdateLesson dto)
		{
			var result = await _context.Lessons
				.FirstOrDefaultAsync(x => x.LessonId == lessonId && x.Course.PublisherUserId == publisherUserId);
			if (result == null) return NotFound();

			_mapper.Map(dto, result);
			await _context.SaveChangesAsync();

			return Ok(_mapper.Map<MaterialResult>(result));
		}


		// DELETE: /lessons/5
		[HttpDelete("{lessonId}")]
		public async Task<ActionResult> Delete(
			[FromHeader] int publisherUserId,
			int lessonId)
		{
			var result = await _context.Lessons
				.Where(x => x.LessonId == lessonId && x.Course.PublisherUserId == publisherUserId)
				.ExecuteDeleteAsync();
			if (result == 0) return NotFound();

			return NoContent();
		}


		// PUT: /lessons/5/order
		[HttpPut("{lessonId}/order")]
		public async Task<ActionResult<LessonResult>> ChangeOrder(
			[FromHeader] int publisherUserId,
			int lessonId,
			ChangeOrder dto)
		{
			var from = await _context.Lessons
				.FirstAsync(x => x.LessonId == lessonId && x.Course.PublisherUserId == publisherUserId);
			if (from == null) return NotFound();

			var to = await _context.Lessons
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.LessonId == dto.ToId);
			if (to == null) return NotFound();

			if (dto.IsBefore)
			{
				var before = await _context.Lessons
					.AsNoTracking()
					.Where(x => x.Order < to.Order)
					.OrderByDescending(x => x.Order)
					.FirstOrDefaultAsync();
				from.OrderNumerator = (before == null ? 0 : before.OrderNumerator) + to.OrderNumerator;
				from.OrderDenominator = (before == null ? 1 : before.OrderDenominator) + to.OrderDenominator;
			}
			else
			{
				var after = await _context.Lessons
					.AsNoTracking()
					.Where(x => x.Order > to.Order)
					.OrderBy(x => x.Order)
					.FirstOrDefaultAsync();
				from.OrderNumerator = (after == null ? (int)Math.Ceiling(to.Order) : after.OrderNumerator) + to.OrderNumerator;
				from.OrderDenominator = (after == null ? 1 : after.OrderDenominator) + to.OrderDenominator;
			}

			await _context.SaveChangesAsync();

			return Ok(_mapper.Map<LessonResult>(from));
		}
	}
}
