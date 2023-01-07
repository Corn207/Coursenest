using AutoMapper;
using Library.API.DTOs.Ratings;
using Library.API.Infrastructure.Contexts;
using Library.API.Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class RatingsController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly DataContext _context;

		public RatingsController(IMapper mapper, DataContext context)
		{
			_mapper = mapper;
			_context = context;
		}


		// GET: /ratings
		[HttpGet()]
		public async Task<ActionResult<IEnumerable<RatingResult>>> GetAll(
			[FromQuery] RatingQuery query)
		{
			var results = await _context.Ratings
				.AsNoTracking()
				.Where(x =>
					(query.CourseId == null || query.CourseId == x.CourseId) &&
					(query.UserId == null || query.UserId == x.UserId))
				.Skip(query.Page * query.PageSize)
				.Take(query.PageSize)
				.Select(x => _mapper.Map<RatingResult>(x))
				.ToListAsync();

			return Ok(results);
		}

		// POST: /ratings
		[HttpPost()]
		public async Task<ActionResult<RatingResult>> Create(
			CreateRating dto)
		{
			var rating = _mapper.Map<Rating>(dto);

			_context.Ratings.Add(rating);
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException)
			{
				return Conflict("Rating existed.");
			}

			var result = _mapper.Map<RatingResult>(rating);

			return result;
		}

		// DELETE: /ratings
		[HttpDelete()]
		public async Task<ActionResult> Delete(
			[FromHeader] int userId,
			int courseId)
		{
			var result = await _context.Ratings
				.Where(x => x.CourseId == courseId && x.UserId == userId)
				.ExecuteDeleteAsync();
			if (result == 0) return NotFound();

			return NoContent();
		}
	}
}
