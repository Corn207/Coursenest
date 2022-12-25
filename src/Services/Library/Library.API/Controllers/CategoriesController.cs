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
    public class CategoriesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CategoriesController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // GET: api/Categories/Full
        [HttpGet("Full")]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetFull()
        {
            return await _context.Categories
                .Include(x => x.Subcategories)
                .ThenInclude(y => y.Topics)
                .Select(x => _mapper.Map<CategoryResponse>(x))
                .ToListAsync();
        }

        // GET: api/Categories
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> Get()
        {
            return await _context.Categories
                .Select(x => _mapper.Map<CategoryResponse>(x))
                .ToListAsync();
        }


        // POST: api/Categories
        [HttpPost]
        public async Task<IActionResult> Post(CategoryRequest request)
        {
            var value = _mapper.Map<Category>(request);
            _context.Categories.Add(value);

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


        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, CategoryRequest request)
        {
            var value = await _context.Categories.FindAsync(id);
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


        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var value = await _context.Categories.FindAsync(id);
            if (value == null) return NotFound();

            _context.Categories.Remove(value);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
