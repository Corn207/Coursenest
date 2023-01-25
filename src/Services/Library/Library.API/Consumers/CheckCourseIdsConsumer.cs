using APICommonLibrary.MessageBus.Commands;
using APICommonLibrary.MessageBus.Responses;
using Library.API.Infrastructure.Contexts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Consumers;

public class CheckCourseIdsConsumer : IConsumer<CheckCourseIds>
{
	private readonly DataContext _context;

	public CheckCourseIdsConsumer(DataContext context)
	{
		_context = context;
	}

	public async Task Consume(ConsumeContext<CheckCourseIds> context)
	{
		var requested = context.Message.Ids;
		var isApproved = context.Message.IsApproved;
		var diff = new List<int>();

		if (requested.Length == 1)
		{
			var exists = await _context.Courses
				.AsNoTracking()
				.AnyAsync(x =>
					x.CourseId == requested[0] &&
					x.IsApproved == isApproved);

			if (!exists)
			{
				diff.Add(requested[0]);
			}

		}
		else if (requested.Length > 1)
		{
			var existed = await _context.Courses
				.AsNoTracking()
				.Where(x => x.IsApproved == isApproved)
				.Select(x => x.CourseId)
				.ToListAsync();

			diff.AddRange(requested.Except(existed));
		}

		if (diff.Any())
		{
			var response = new NotFound() { Message = $"Topic IDs: {string.Join(", ", diff)} not existed." };
			await context.RespondAsync(response);
		}
		else
		{
			var response = new Existed();
			await context.RespondAsync(response);
		}
	}
}
