using AutoMapper;
using Library.API.DTOs;
using Library.API.DTOs.Units;
using Library.API.Infrastructure.Contexts;
using Library.API.Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class UnitsController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly DataContext _context;

		public UnitsController(IMapper mapper, DataContext context)
		{
			_mapper = mapper;
			_context = context;
		}


		private async Task<bool> IsLessonOwned(int lessonId, int publisherUserId)
		{
			var result = await _context.Lessons
				.AsNoTracking()
				.AnyAsync(x => x.LessonId == lessonId && x.Course.PublisherUserId == publisherUserId);

			return result;
		}

		// GET: /units
		[HttpGet()]
		public async Task<ActionResult<IEnumerable<UnitResult>>> GetAll(
			[FromHeader] int? publisherUserId,
			int lessonId)
		{
			IEnumerable<UnitResult> results;
			if (publisherUserId == null)
			{
				results = await _context.Units
					.AsNoTracking()
					.Where(x => x.LessonId == lessonId && x.Lesson.Course.IsApproved)
					.Select(x => _mapper.Map<UnitResult>(x))
					.ToListAsync();
			}
			else
			{
				if (await IsLessonOwned(lessonId, (int)publisherUserId)) return NotFound();

				results = await _context.Units
					.AsNoTracking()
					.Where(x => x.LessonId == lessonId)
					.Select(x => _mapper.Map<UnitResult>(x))
					.ToListAsync();
			}

			return Ok(results);
		}

		// GET: /units/5
		[HttpGet("{unitId}")]
		public async Task<IActionResult> Get(
			[FromHeader] int? publisherUserId,
			int unitId)
		{
			Unit? unit;
			if (publisherUserId == null)
			{
				unit = await _context.Units
					.AsNoTracking()
					.Where(x => x.UnitId == unitId && x.Lesson.Course.IsApproved)
					.FirstOrDefaultAsync();
			}
			else
			{
				unit = await _context.Units
					.AsNoTracking()
					.Where(x => x.UnitId == unitId && x.Lesson.Course.PublisherUserId == (int)publisherUserId)
					.FirstOrDefaultAsync();
			}
			if (unit == null) return NotFound();

			if (unit is Material material)
			{
				var result = _mapper.Map<MaterialResult>(material);

				return Ok(result);
			}
			else
			{
				var exam = unit as Exam;

				await _context.Entry(exam!)
					.Collection(x => x.Questions)
					.Query()
					.Include(x => x.Choices)
					.LoadAsync();

				var result = _mapper.Map<ExamResult>(exam);

				return Ok(result);
			}
		}


		// POST: /units/material
		[HttpPost("material")]
		public async Task<ActionResult<MaterialResult>> CreateMaterial(
			[FromHeader] int publisherUserId,
			CreateMaterial dto)
		{
			if (await IsLessonOwned(dto.LessonId, publisherUserId))
				return NotFound();

			var result = _mapper.Map<Material>(dto);

			var max = await _context.Units
				.Where(x => x.LessonId == dto.LessonId)
				.OrderByDescending(x => x.Order)
				.Select(x => x.Order)
				.FirstOrDefaultAsync();
			result.OrderNumerator = max == default ? 1 : (int)Math.Ceiling(max);
			result.OrderDenominator = 1;

			_context.Materials.Add(result);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(Get), new { publisherUserId, result.UnitId }, _mapper.Map<MaterialResult>(result));
		}

		// POST: /units/exam
		[HttpPost("exam")]
		public async Task<ActionResult<ExamResult>> CreateExam(
			[FromHeader] int publisherUserId,
			CreateExam dto)
		{
			if (await IsLessonOwned(dto.LessonId, publisherUserId))
				return NotFound();

			var result = _mapper.Map<Exam>(dto);

			var max = await _context.Units
				.Where(x => x.LessonId == dto.LessonId)
				.OrderByDescending(x => x.Order)
				.Select(x => x.Order)
				.FirstOrDefaultAsync();
			result.OrderNumerator = max == default ? 1 : (int)Math.Ceiling(max);
			result.OrderDenominator = 1;

			_context.Exams.Add(result);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(Get), new { publisherUserId, result.UnitId }, _mapper.Map<ExamResult>(result));
		}


		// PUT: /units/5/material
		[HttpPut("{unitId}/material")]
		public async Task<ActionResult<MaterialResult>> UpdateMaterial(
			[FromHeader] int publisherUserId,
			int unitId,
			UpdateMaterial dto)
		{
			var result = await _context.Materials
				.FirstOrDefaultAsync(x => x.UnitId == unitId && x.Lesson.Course.PublisherUserId == publisherUserId);
			if (result == null) return NotFound();

			_mapper.Map(dto, result);
			await _context.SaveChangesAsync();

			return Ok(_mapper.Map<MaterialResult>(result));
		}

		// PUT: /units/5/exam
		[HttpPut("{unitId}/exam")]
		public async Task<ActionResult<ExamResult>> UpdateExam(
			[FromHeader] int publisherUserId,
			int unitId,
			UpdateExam dto)
		{
			var result = await _context.Exams
				.FirstOrDefaultAsync(x => x.UnitId == unitId && x.Lesson.Course.PublisherUserId == publisherUserId);
			if (result == null) return NotFound();

			_mapper.Map(dto, result);
			await _context.SaveChangesAsync();

			return Ok(_mapper.Map<ExamResult>(result));
		}


		// DELETE: /units/5
		[HttpDelete("{unitId}")]
		public async Task<ActionResult> Delete(
			[FromHeader] int publisherUserId,
			int unitId)
		{
			var result = await _context.Units
				.Where(x => x.UnitId == unitId && x.Lesson.Course.PublisherUserId == publisherUserId)
				.ExecuteDeleteAsync();
			if (result == 0) return NotFound();

			return NoContent();
		}


		// PUT: /units/5/order
		[HttpPut("{unitId}/order")]
		public async Task<ActionResult<UnitResult>> ChangeOrder(
			[FromHeader] int publisherUserId,
			int unitId,
			ChangeOrder dto)
		{
			var from = await _context.Units
				.FirstOrDefaultAsync(x => x.UnitId == unitId && x.Lesson.Course.PublisherUserId == publisherUserId);
			if (from == null) return NotFound();

			var to = await _context.Units
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.UnitId == dto.ToId);
			if (to == null) return NotFound();

			if (dto.IsBefore)
			{
				var before = await _context.Units
					.AsNoTracking()
					.Where(x => x.Order < to.Order)
					.OrderByDescending(x => x.Order)
					.FirstOrDefaultAsync();
				from.OrderNumerator = (before == null ? 0 : before.OrderNumerator) + to.OrderNumerator;
				from.OrderDenominator = (before == null ? 1 : before.OrderDenominator) + to.OrderDenominator;
			}
			else
			{
				var after = await _context.Units
					.AsNoTracking()
					.Where(x => x.Order > to.Order)
					.OrderBy(x => x.Order)
					.FirstOrDefaultAsync();
				from.OrderNumerator = (after == null ? (int)Math.Ceiling(to.Order) : after.OrderNumerator) + to.OrderNumerator;
				from.OrderDenominator = (after == null ? 1 : after.OrderDenominator) + to.OrderDenominator;
			}

			await _context.SaveChangesAsync();

			return Ok(_mapper.Map<UnitResult>(from));
		}


		// POST: /units/5/questions
		[HttpPost("{examUnitId}/questions")]
		public async Task<ActionResult<QuestionResult>> CreateQuestion(
			[FromHeader] int publisherUserId,
			CreateQuestion dto)
		{
			var exam = await _context.Exams
				.FirstOrDefaultAsync(x => x.UnitId == dto.ExamUnitId && x.Lesson.Course.PublisherUserId == publisherUserId);
			if (exam == null) return NotFound();

			var result = _mapper.Map<Question>(dto);

			exam.Questions.Add(result);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(Get), new { publisherUserId, result.ExamUnitId }, _mapper.Map<QuestionResult>(result));
		}


		// PUT: /units/5/questions/5
		[HttpPut("{examUnitId}/questions/{questionId}")]
		public async Task<ActionResult<QuestionResult>> UpdateQuestion(
			[FromHeader] int publisherUserId,
			int examUnitId,
			int questionId,
			UpdateQuestion dto)
		{
			var result = await _context.Questions
				.FirstOrDefaultAsync(x =>
					x.QuestionId == questionId &&
					x.ExamUnitId == examUnitId &&
					x.Exam.Lesson.Course.PublisherUserId == publisherUserId);
			if (result == null) return NotFound();

			_mapper.Map(dto, result);

			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(Get), new { publisherUserId, result.ExamUnitId }, _mapper.Map<QuestionResult>(result));
		}
	}
}
