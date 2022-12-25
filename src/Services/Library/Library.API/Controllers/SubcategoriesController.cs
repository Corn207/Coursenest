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
    public class SubcategoriesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public SubcategoriesController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // GET: api/Subcategories/Full
        [HttpGet("Full")]
        public async Task<ActionResult<IEnumerable<SubcategoryResponse>>> GetFull()
        {
            return await _context.Subcategories
                .Include(x => x.Topics)
                .Select(x => _mapper.Map<SubcategoryResponse>(x))
                .ToListAsync();
        }

        // GET: api/Subcategories
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<SubcategoryResponse>>> Get()
        {
            return await _context.Subcategories
                .Select(x => _mapper.Map<SubcategoryResponse>(x))
                .ToListAsync();
        }


        // POST: api/Subcategories
        [HttpPost]
        public async Task<IActionResult> Post(SubcategoryRequest request)
        {
            var value = _mapper.Map<Subcategory>(request);
            _context.Subcategories.Add(value);

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


        // PUT: api/Subcategories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, SubcategoryRequest request)
        {
            var value = await _context.Subcategories.FindAsync(id);
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


        // DELETE: api/Subcategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var value = await _context.Subcategories.FindAsync(id);
            if (value == null) return NotFound();

            _context.Subcategories.Remove(value);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
