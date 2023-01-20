using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserData.API.DTOs;
using UserData.API.Infrastructure.Contexts;

namespace UserData.API.Controllers;

[Route("[controller]")]
[ApiController]
public class SubmissionsController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly DataContext _context;

	public SubmissionsController(IMapper mapper, DataContext context)
	{
		_mapper = mapper;
		_context = context;
	}


	// POST: /submissions/submit
	[HttpPost]
	public async Task<ActionResult> Submit(
		[FromHeader] int userId,
		[FromBody] IEnumerable<Answer> body)
	{
		//var isOwned = await _context.Courses
		//	.AsNoTracking()
		//	.AnyAsync(x => x.CourseId == dto.CourseId && x.PublisherUserId == publisherUserId);
		//if (!isOwned) return NotFound();

		//var lesson = _mapper.Map<Lesson>(dto);

		//var max = await _context.Lessons
		//	.AsNoTracking()
		//	.Where(x => x.CourseId == dto.CourseId)
		//	.OrderByDescending(x => x.Order)
		//	.Select(x => x.Order)
		//	.FirstOrDefaultAsync();
		//lesson.OrderNumerator = max == default ? 1 : (int)Math.Ceiling(max);
		//lesson.OrderDenominator = 1;

		//_context.Lessons.Add(lesson);
		//await _context.SaveChangesAsync();

		//var result = _mapper.Map<LessonResult>(lesson);

		return NoContent();
	}
}
