using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library.API.Contexts;
using Library.API.Models;
using AutoMapper;
using Library.API.DTOs.Courses;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class CoursesController : ControllerBase
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public CoursesController(DataContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}


		// GET: api/Courses
		[HttpGet]
		public async Task<ActionResult<IEnumerable<CourseCardResponse>>> GetCourses([FromQuery] CoursesQueries queries)
		{
			return await _context.Courses
				.Include(x => x.Image)
				.Include(x => x.Ratings)
				.Where(x => queries.TopicIds.Contains(x.TopicId))
				.Where(x => queries.PublisherUserId == null || x.PublisherUserId == queries.PublisherUserId)
				.OrderByDescending(x => x.Ratings.Count)
				.Take(queries.Top)
				.Select(x => _mapper.Map<CourseCardResponse>(x))
				.ToListAsync();
		}

		// GET: api/Courses/5
		[HttpGet("{id}")]
		public async Task<ActionResult<CourseResponse>> GetById(int id)
		{
			var course = await _context.Courses
				.Include(x => x.Lessons)
				.ThenInclude(x => x.Units)
				.FirstOrDefaultAsync(x => x.CourseId == id);
			if (course == null) return NotFound();

            return _mapper.Map<CourseResponse>(course);
		}


		// PUT: api/Courses/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutCourse(int id, Course course)
		{
			if (id != course.CourseId)
			{
				return BadRequest();
			}

			_context.Entry(course).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CourseExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// POST: api/Courses
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Course>> PostCourse(Course course)
		{
			_context.Courses.Add(course);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCourse", new { id = course.CourseId }, course);
		}


		// DELETE: api/Courses/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCourse(int id)
		{
			var course = await _context.Courses.FindAsync(id);
			if (course == null) return NotFound();

			_context.Courses.Remove(course);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool CourseExists(int id)
		{
			return _context.Courses.Any(e => e.CourseId == id);
		}
	}
}
