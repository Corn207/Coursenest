using AutoMapper;
using Library.API.DTOs.Categories;
using Library.API.Infrastructure.Contexts;
using Library.API.Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly DataContext _context;

		public CategoriesController(IMapper mapper, DataContext context)
		{
			_mapper = mapper;
			_context = context;
		}


		// GET: /categories/hierarchy
		[HttpGet("hierarchy")]
		public async Task<ActionResult<IEnumerable<CategoryResult>>> GetAllHierarchy()
		{
			var results = await _context.Categories
				.AsNoTracking()
				.Include(x => x.Subcategories)
				.ThenInclude(x => x.Topics)
				.ToListAsync();

			return Ok(results.Select(x => _mapper.Map<CategoryResult>(x)));
		}


		// GET: /categories
		[HttpGet()]
		public async Task<ActionResult<IEnumerable<CategoryResult>>> GetAll()
		{
			var results = await _context.Categories
				.AsNoTracking()
				.ToListAsync();

			return Ok(results.Select(x => _mapper.Map<CategoryResult>(x)));
		}


		// GET: /categories/5
		[HttpGet("{categoryId}")]
		public async Task<ActionResult<CategoryResult>> Get(int categoryId)
		{
			var result = await _context.Categories
				.AsNoTracking()
				.Include(x => x.Subcategories)
				.FirstOrDefaultAsync(x => x.CategoryId == categoryId);

			return _mapper.Map<CategoryResult>(result);
		}


		// POST: /categories
		[HttpPost]
		public async Task<ActionResult> Create(string content)
		{
			var result = new Category() { Content = content };

			_context.Categories.Add(result);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(Get), result.CategoryId, result);
		}


		// PUT: /categories/5
		[HttpPut("{categoryId}")]
		public async Task<ActionResult> Update(int categoryId, string content)
		{
			var result = await _context.Categories.FindAsync(categoryId);
			if (result == null) return NotFound();

			result.Content = content;
			await _context.SaveChangesAsync();

			return NoContent();
		}


		// DELETE: /categories/5
		[HttpDelete("{categoryId}")]
		public async Task<ActionResult> Delete(int categoryId)
		{
			var result = await _context.Categories
				.Where(x => x.CategoryId == categoryId)
				.ExecuteDeleteAsync();
			if (result == 0) return NotFound();

			return NoContent();
		}




		// GET: /subcategories
		[HttpGet("/subcategories")]
		public async Task<ActionResult<IEnumerable<SubcategoryResult>>> GetAllSubcategory()
		{
			var results = await _context.Subcategories
				.AsNoTracking()
				.ToListAsync();

			return Ok(results.Select(x => _mapper.Map<SubcategoryResult>(x)));
		}


		// GET: /subcategories/5
		[HttpGet("/subcategories/{subcategoryId}")]
		public async Task<ActionResult<SubcategoryDetailedResult>> GetSubcategory(int subcategoryId)
		{
			var result = await _context.Subcategories
				.AsNoTracking()
				.Include(x => x.Category)
				.Include(x => x.Topics)
				.FirstOrDefaultAsync(x => x.SubcategoryId == subcategoryId);

			return _mapper.Map<SubcategoryDetailedResult>(result);
		}


		// POST: /subcategories
		[HttpPost("/subcategories")]
		public async Task<ActionResult> CreateSubcategory(CreateSubcategory dto)
		{
			var result = new Subcategory() { Content = dto.Content, CategoryId = dto.CategoryId };

			_context.Subcategories.Add(result);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetSubcategory), result.SubcategoryId, result);
		}


		// PUT: /subcategories/5
		[HttpPut("/subcategories/{subcategoryId}")]
		public async Task<ActionResult> UpdateSubcategory(int subcategoryId, string content)
		{
			var result = await _context.Subcategories.FindAsync(subcategoryId);
			if (result == null) return NotFound();

			result.Content = content;
			await _context.SaveChangesAsync();

			return NoContent();
		}


		// DELETE: /subcategories/5
		[HttpDelete("/subcategories/{subcategoryId}")]
		public async Task<ActionResult> DeleteSubcategory(int subcategoryId)
		{
			var result = await _context.Subcategories
				.Where(x => x.SubcategoryId == subcategoryId)
				.ExecuteDeleteAsync();
			if (result == 0) return NotFound();

			return NoContent();
		}




		// GET: /topics
		[HttpGet("/topics")]
		public async Task<ActionResult<IEnumerable<TopicResult>>> GetAllTopic()
		{
			var results = await _context.Topics
				.AsNoTracking()
				.ToListAsync();

			return Ok(results.Select(x => _mapper.Map<TopicResult>(x)));
		}


		// GET: /topics/5
		[HttpGet("/topics/{topicId}")]
		public async Task<ActionResult<TopicDetailedResult>> GetTopic(int topicId)
		{
			var result = await _context.Topics
				.AsNoTracking()
				.Include(x => x.Subcategory)
				.ThenInclude(x => x.Category)
				.FirstOrDefaultAsync(x => x.TopicId == topicId);

			return _mapper.Map<TopicDetailedResult>(result);
		}


		// POST: /topics
		[HttpPost("/topics")]
		public async Task<ActionResult> CreateTopic(CreateTopic dto)
		{
			var result = new Topic() { Content = dto.Content, SubcategoryId = dto.SubcategoryId };

			_context.Topics.Add(result);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetTopic), result.TopicId, result);
		}


		// PUT: /topics/5
		[HttpPut("/topics/{topicId}")]
		public async Task<ActionResult> UpdateTopic(int topicId, string content)
		{
			var result = await _context.Topics.FindAsync(topicId);
			if (result == null) return NotFound();

			result.Content = content;
			await _context.SaveChangesAsync();

			return NoContent();
		}


		// DELETE: /topics/5
		[HttpDelete("/topics/{topicId}")]
		public async Task<ActionResult> DeleteTopic(int topicId)
		{
			var result = await _context.Topics
				.Where(x => x.TopicId == topicId)
				.ExecuteDeleteAsync();
			if (result == 0) return NotFound();

			return NoContent();
		}
	}
}
