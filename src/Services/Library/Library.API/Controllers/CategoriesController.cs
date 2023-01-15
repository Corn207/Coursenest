using AutoMapper;
using AutoMapper.QueryableExtensions;
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
				.ProjectTo<CategoryResult>(_mapper.ConfigurationProvider)
				.ToListAsync();

			return results;
		}

		// GET: /categories
		[HttpGet()]
		public async Task<ActionResult<IEnumerable<IdContentResult>>> GetAll()
		{
			var results = await _context.Categories
				.AsNoTracking()
				.ProjectTo<IdContentResult>(_mapper.ConfigurationProvider)
				.ToListAsync();

			return results;
		}

		// GET: /categories/5
		[HttpGet("{categoryId}")]
		public async Task<ActionResult<IdContentResult>> Get(
			int categoryId)
		{
			var result = await _context.Categories
				.AsNoTracking()
				.ProjectTo<IdContentResult>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(x => x.Id == categoryId);
			if (result == null) return NotFound();

			return result;
		}


		// POST: /categories
		[HttpPost]
		public async Task<ActionResult<IdContentResult>> Create(
			[FromBody] string content)
		{
			var category = new Category() { Content = content };

			_context.Categories.Add(category);
			await _context.SaveChangesAsync();

			var result = _mapper.Map<IdContentResult>(category);

			return CreatedAtAction(nameof(Get), new { category.CategoryId }, result);
		}


		// PUT: /categories/5
		[HttpPut("{categoryId}")]
		public async Task<ActionResult<IdContentResult>> Update(
			int categoryId,
			[FromBody] string content)
		{
			var category = await _context.Categories.FindAsync(categoryId);
			if (category == null) return NotFound();

			category.Content = content;
			await _context.SaveChangesAsync();

			var result = _mapper.Map<IdContentResult>(category);

			return result;
		}


		// DELETE: /categories/5
		[HttpDelete("{categoryId}")]
		public async Task<ActionResult> Delete(
			int categoryId)
		{
			var result = await _context.Categories
				.Where(x => x.CategoryId == categoryId)
				.ExecuteDeleteAsync();
			if (result == 0) return NotFound();

			return NoContent();
		}



		// GET: /subcategories
		[HttpGet("/subcategories")]
		public async Task<ActionResult<IEnumerable<IdContentResult>>> GetAllSubcategory(
			[FromQuery] int categoryId)
		{
			var results = await _context.Subcategories
				.AsNoTracking()
				.Where(x => x.CategoryId == categoryId)
				.ProjectTo<IdContentResult>(_mapper.ConfigurationProvider)
				.ToListAsync();

			return results;
		}

		// GET: /subcategories/5
		[HttpGet("/subcategories/{subcategoryId}")]
		public async Task<ActionResult<IdContentResult>> GetSubcategory(
			int subcategoryId)
		{
			var result = await _context.Subcategories
				.AsNoTracking()
				.ProjectTo<IdContentResult>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(x => x.Id == subcategoryId);
			if (result == null) return NotFound();

			return result;
		}


		// POST: /subcategories
		[HttpPost("/subcategories")]
		public async Task<ActionResult<IdContentResult>> CreateSubcategory(
			CreateParentIdContent dto)
		{
			var subcategory = new Subcategory() { Content = dto.Content, CategoryId = dto.ParentId };

			_context.Subcategories.Add(subcategory);
			await _context.SaveChangesAsync();

			var result = _mapper.Map<IdContentResult>(subcategory);

			return CreatedAtAction(nameof(GetSubcategory), new { subcategory.SubcategoryId }, result);
		}


		// PUT: /subcategories/5
		[HttpPut("/subcategories/{subcategoryId}")]
		public async Task<ActionResult<IdContentResult>> UpdateSubcategory(
			int subcategoryId,
			[FromBody] string content)
		{
			var subcategory = await _context.Subcategories.FindAsync(subcategoryId);
			if (subcategory == null) return NotFound();

			subcategory.Content = content;
			await _context.SaveChangesAsync();

			var result = _mapper.Map<IdContentResult>(subcategory);

			return result;
		}


		// DELETE: /subcategories/5
		[HttpDelete("/subcategories/{subcategoryId}")]
		public async Task<ActionResult> DeleteSubcategory(
			int subcategoryId)
		{
			var result = await _context.Subcategories
				.Where(x => x.SubcategoryId == subcategoryId)
				.ExecuteDeleteAsync();
			if (result == 0) return NotFound();

			return NoContent();
		}



		// GET: /topics
		[HttpGet("/topics")]
		public async Task<ActionResult<IEnumerable<IdContentResult>>> GetAllTopic(
			[FromQuery] int subcategoryId)
		{
			var results = await _context.Topics
				.AsNoTracking()
				.Where(x => x.SubcategoryId == subcategoryId)
				.ProjectTo<IdContentResult>(_mapper.ConfigurationProvider)
				.ToListAsync();

			return results;
		}

		// GET: /topics/5
		[HttpGet("/topics/{topicId}")]
		public async Task<ActionResult<TopicDetailedResult>> GetTopic(
			int topicId)
		{
			var result = await _context.Topics
				.AsNoTracking()
				.ProjectTo<TopicDetailedResult>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(x => x.TopicId == topicId);
			if (result == null) return NotFound();

			return result;
		}


		// POST: /topics
		[HttpPost("/topics")]
		public async Task<ActionResult<IdContentResult>> CreateTopic(
			CreateParentIdContent dto)
		{
			var topic = new Topic() { Content = dto.Content, SubcategoryId = dto.ParentId };

			_context.Topics.Add(topic);
			await _context.SaveChangesAsync();

			var result = _mapper.Map<IdContentResult>(topic);

			return CreatedAtAction(nameof(GetTopic), new { topic.TopicId }, result);
		}


		// PUT: /topics/5
		[HttpPut("/topics/{topicId}")]
		public async Task<ActionResult<IdContentResult>> UpdateTopic(
			int topicId,
			[FromBody] string content)
		{
			var topic = await _context.Topics.FindAsync(topicId);
			if (topic == null) return NotFound();

			topic.Content = content;
			await _context.SaveChangesAsync();

			var result = _mapper.Map<IdContentResult>(topic);

			return result;
		}


		// DELETE: /topics/5
		[HttpDelete("/topics/{topicId}")]
		public async Task<ActionResult> DeleteTopic(
			int topicId)
		{
			var test = await _context.Topics.ToListAsync();

			var result = await _context.Topics
				.Where(x => x.TopicId == topicId)
				.ExecuteDeleteAsync();
			if (result == 0) return NotFound();

			return NoContent();
		}
	}
}
