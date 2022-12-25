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
using Library.API.DTOs.Ratings;
using System.ComponentModel.DataAnnotations;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public RatingsController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // GET: api/Ratings
        [HttpGet(), Authorize]
        public async Task<ActionResult<IEnumerable<RatingResponse>>> GetByCourseId(
            [FromQuery][Required] int courseId, [FromHeader] string test)
        {
            Console.WriteLine(Request.Headers);
            var ratings = await _context.Ratings
                .Where(x => x.CourseId == courseId)
                .ToListAsync();


            return Ok(ratings.Select(x => _mapper.Map<RatingResponse>(x)));
        }

        // GET: api/Ratings/Stat
        [HttpGet("Stat")]
        public async Task<ActionResult<RatingStatResponse>> GetStatByCourseId(
            [FromQuery][Required] int courseId)
        {
            var ratings = await _context.Ratings
                .Where(x => x.CourseId == courseId)
                .ToListAsync();

            return _mapper.Map<RatingStatResponse>(ratings);
        }

        // GET: api/Ratings/5
        [HttpGet("{userId}")]
        public async Task<ActionResult<Rating>> Get(
            int userId,
            [FromQuery][Required] int courseId)
        {
            var rating = await _context.Ratings
                .FirstOrDefaultAsync(x => x.CourseId == courseId && x.UserId == userId);
            if (rating == null) return NotFound();

            return rating;
        }


        // POST: api/Ratings/5
        [HttpPost("{userId}")]
        public async Task<ActionResult<Rating>> PostRating(
            int userId,
            [FromQuery][Required] int courseId,
            RatingPostRequest request)
        {
            var rating = _mapper.Map<Rating>(request);
            rating.UserId = userId;
            rating.Created= DateTime.Now;
            _context.Ratings.Add(rating);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (_context.Ratings.Any(x => x.CourseId == courseId && x.UserId == userId))
                {
                    return Conflict(new { Message = $"Existed Rating record with userId ({userId}), courseId ({courseId})." });
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(Get), new { CourseId = rating.CourseId, UserId = userId });
        }


        // PUT: api/Ratings/5
        [HttpPut("{userId}")]
        public async Task<IActionResult> PutRating(int userId, RatingPutRequest request)
        {
            var rating = await _context.Ratings
                .FirstOrDefaultAsync(x => x.CourseId == request.CourseId && x.UserId == userId);
            if (rating == null) return NotFound();

            _mapper.Map(request, rating);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return NoContent();
        }


        // DELETE: api/Ratings/5
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteRating(
            int userId,
            [FromQuery][Required] int courseId)
        {
            var rating = await _context.Ratings
                .FirstOrDefaultAsync(x => x.CourseId == courseId && x.UserId == userId);
            if (rating == null) return NotFound();

            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
