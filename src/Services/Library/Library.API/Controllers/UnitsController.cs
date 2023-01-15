using AutoMapper;
using AutoMapper.QueryableExtensions;
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
			List<UnitResult> results;
			if (publisherUserId == null)
			{
				results = await _context.Units
					.AsNoTracking()
					.Where(x => x.LessonId == lessonId && x.Lesson.Course.IsApproved)
					.ProjectTo<UnitResult>(_mapper.ConfigurationProvider)
					.ToListAsync();
			}
			else
			{
				results = await _context.Units
					.AsNoTracking()
					.Where(x => x.LessonId == lessonId && x.Lesson.Course.PublisherUserId == publisherUserId)
					.ProjectTo<UnitResult>(_mapper.ConfigurationProvider)
					.ToListAsync();
			}

			return results;
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
					.FirstOrDefaultAsync(x => x.UnitId == unitId && x.Lesson.Course.IsApproved);
			}
			else
			{
				unit = await _context.Units
					.AsNoTracking()
					.FirstOrDefaultAsync(x => x.UnitId == unitId && x.Lesson.Course.PublisherUserId == publisherUserId);
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
			var exists = await _context.Lessons
				.AsNoTracking()
				.AnyAsync(x => x.LessonId == dto.LessonId && x.Course.PublisherUserId == publisherUserId);
			if (!exists) return NotFound();

			var material = _mapper.Map<Material>(dto);

			var max = await _context.Units
				.AsNoTracking()
				.Where(x => x.LessonId == dto.LessonId)
				.OrderByDescending(x => x.Order)
				.Select(x => x.Order)
				.FirstOrDefaultAsync();
			material.OrderNumerator = max == default ? 1 : (int)Math.Ceiling(max);
			material.OrderDenominator = 1;

			_context.Materials.Add(material);
			await _context.SaveChangesAsync();

			var result = _mapper.Map<MaterialResult>(material);

			return CreatedAtAction(nameof(Get), new { publisherUserId, material.UnitId }, result);
		}

		// POST: /units/exam
		[HttpPost("exam")]
		public async Task<ActionResult<ExamResult>> CreateExam(
			[FromHeader] int publisherUserId,
			CreateExam dto)
		{
			var exists = await _context.Lessons
				.AsNoTracking()
				.AnyAsync(x => x.LessonId == dto.LessonId && x.Course.PublisherUserId == publisherUserId);
			if (!exists) return NotFound();

			var exam = _mapper.Map<Exam>(dto);

			var max = await _context.Units
				.AsNoTracking()
				.Where(x => x.LessonId == dto.LessonId)
				.OrderByDescending(x => x.Order)
				.Select(x => x.Order)
				.FirstOrDefaultAsync();
			exam.OrderNumerator = max == default ? 1 : (int)Math.Ceiling(max);
			exam.OrderDenominator = 1;

			_context.Exams.Add(exam);
			await _context.SaveChangesAsync();

			var result = _mapper.Map<ExamResult>(exam);

			return CreatedAtAction(nameof(Get), new { publisherUserId, exam.UnitId }, exam);
		}


		// PUT: /units/5/material
		[HttpPut("{unitId}/material")]
		public async Task<ActionResult<MaterialResult>> UpdateMaterial(
			[FromHeader] int publisherUserId,
			int unitId,
			UpdateMaterial dto)
		{
			var material = await _context.Materials
				.FirstOrDefaultAsync(x => x.UnitId == unitId && x.Lesson.Course.PublisherUserId == publisherUserId);
			if (material == null) return NotFound();

			_mapper.Map(dto, material);
			await _context.SaveChangesAsync();

			var result = _mapper.Map<MaterialResult>(material);

			return result;
		}

		// PUT: /units/5/exam
		[HttpPut("{unitId}/exam")]
		public async Task<ActionResult<ExamResult>> UpdateExam(
			[FromHeader] int publisherUserId,
			int unitId,
			UpdateExam dto)
		{
			var exam = await _context.Exams
				.FirstOrDefaultAsync(x => x.UnitId == unitId && x.Lesson.Course.PublisherUserId == publisherUserId);
			if (exam == null) return NotFound();

			_mapper.Map(dto, exam);
			await _context.SaveChangesAsync();

			var result = _mapper.Map<ExamResult>(exam);

			return result;
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
				.FirstOrDefaultAsync(x => x.UnitId == dto.ToId && x.LessonId == from.LessonId);
			if (to == null) return NotFound();

			if (dto.IsBefore)
			{
				var before = await _context.Units
					.AsNoTracking()
					.Where(x => x.Order < to.Order && x.LessonId == from.LessonId)
					.OrderByDescending(x => x.Order)
					.FirstOrDefaultAsync();
				from.OrderNumerator = (before == null ? 0 : before.OrderNumerator) + to.OrderNumerator;
				from.OrderDenominator = (before == null ? 1 : before.OrderDenominator) + to.OrderDenominator;
			}
			else
			{
				var after = await _context.Units
					.AsNoTracking()
					.Where(x => x.Order > to.Order && x.LessonId == from.LessonId)
					.OrderBy(x => x.Order)
					.FirstOrDefaultAsync();
				from.OrderNumerator = (after == null ? (int)Math.Ceiling(to.Order) : after.OrderNumerator) + to.OrderNumerator;
				from.OrderDenominator = (after == null ? 1 : after.OrderDenominator) + to.OrderDenominator;
			}

			await _context.SaveChangesAsync();

			var result = _mapper.Map<UnitResult>(from);

			return result;
		}


		// POST: /units/5/questions
		[HttpPost("{examUnitId}/questions")]
		public async Task<ActionResult<QuestionResult>> CreateQuestion(
			[FromHeader] int publisherUserId,
			CreateQuestion dto)
		{
			var exists = await _context.Exams
				.AsNoTracking()
				.AnyAsync(x => x.UnitId == dto.ExamUnitId && x.Lesson.Course.PublisherUserId == publisherUserId);
			if (!exists) return NotFound();

			var question = _mapper.Map<Question>(dto);

			_context.Questions.Add(question);
			await _context.SaveChangesAsync();

			var result = _mapper.Map<QuestionResult>(question);

			return CreatedAtAction(nameof(Get), new { publisherUserId, question.ExamUnitId }, result);
		}


		// PUT: /units/5/questions/5
		[HttpPut("{examUnitId}/questions/{questionId}")]
		public async Task<ActionResult<QuestionResult>> UpdateQuestion(
			[FromHeader] int publisherUserId,
			int examUnitId,
			int questionId,
			UpdateQuestion dto)
		{
			var question = await _context.Questions
				.Include(x => x.Choices)
				.FirstOrDefaultAsync(x =>
					x.QuestionId == questionId &&
					x.ExamUnitId == examUnitId &&
					x.Exam.Lesson.Course.PublisherUserId == publisherUserId);
			if (question == null) return NotFound();

			_mapper.Map(dto, question);

			await _context.SaveChangesAsync();

			var result = _mapper.Map<QuestionResult>(question);

			return CreatedAtAction(nameof(Get), new { publisherUserId, question.ExamUnitId }, result);
		}


		// DELETE: /units/5/questions/5
		[HttpDelete("{examUnitId}/questions/{questionId}")]
		public async Task<ActionResult> DeleteQuestion(
			[FromHeader] int publisherUserId,
			int examUnitId,
			int questionId)
		{
			var result = await _context.Questions
				.Where(x =>
					x.QuestionId == questionId &&
					x.Exam.UnitId == examUnitId &&
					x.Exam.Lesson.Course.PublisherUserId == publisherUserId)
				.ExecuteDeleteAsync();
			if (result == 0) return NotFound();

			return NoContent();
		}
	}
}
