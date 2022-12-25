using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library.API.Contexts;
using Library.API.Models;
using Library.API.DTOs;
using AutoMapper;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public TopicsController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // GET: api/Topics
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<TopicRequest>>> Get()
        {
            return await _context.Topics
                .Select(x => _mapper.Map<TopicRequest>(x))
                .ToListAsync();
        }


        // POST: api/Topics
        [HttpPost]
        public async Task<IActionResult> Post(TopicRequest request)
        {
            var value = _mapper.Map<Topic>(request);
            _context.Topics.Add(value);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Created("", null);
        }


        // PUT: api/Topics/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, TopicRequest request)
        {
            var value = await _context.Topics.FindAsync(id);
            if (value == null) return NotFound();

            _mapper.Map(request, value);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }


        // DELETE: api/Topics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var value = await _context.Topics.FindAsync(id);
            if (value == null) return NotFound();

            _context.Topics.Remove(value);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
