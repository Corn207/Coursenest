using APICommonLibrary.MessageBus.Commands;
using APICommonLibrary.MessageBus.Responses;
using Library.API.Infrastructure.Contexts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Consumers;

public class CheckTopicsConsumer : IConsumer<CheckTopics>
{
	private readonly DataContext _context;

	public CheckTopicsConsumer(DataContext context)
	{
		_context = context;
	}

	public async Task Consume(ConsumeContext<CheckTopics> context)
	{
		var requested = context.Message.TopicIds;

		if (requested.Length == 1)
		{
			var exists = await _context.Topics
				.AsNoTracking()
				.AnyAsync(x => x.TopicId == requested[0]);

			if (!exists)
			{
				var response = new NotFound() { Message = $"Topic IDs: {requested[0]} not existed." };
				await context.RespondAsync(response);

				return;
			}

		}
		else if (requested.Length > 1)
		{
			var existed = await _context.Topics
				.AsNoTracking()
				.Select(x => x.TopicId)
				.ToListAsync();

			var diff = requested.Except(existed);

			if (diff.Any())
			{
				var response = new NotFound() { Message = $"Topic IDs: {string.Join(", ", diff)} not existed." };
				await context.RespondAsync(response);

				return;
			}
		}

		await context.RespondAsync(new Existed());
	}
}
