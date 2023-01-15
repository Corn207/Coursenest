using APICommonLibrary.MessageBus.Commands;
using APICommonLibrary.MessageBus.Responses;
using Identity.API.Infrastructure.Contexts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Consumers;

public class CheckUserEmailsConsumer : IConsumer<CheckUserEmails>
{
	private readonly DataContext _context;

	public CheckUserEmailsConsumer(DataContext context)
	{
		_context = context;
	}

	public async Task Consume(ConsumeContext<CheckUserEmails> context)
	{
		var requested = context.Message.Emails;

		if (requested.Length == 1)
		{
			var exists = await _context.Users
				.AsNoTracking()
				.AnyAsync(x => x.Email == requested[0]);

			if (!exists)
			{
				var response = new NotFound() { Message = $"Emails: {requested[0]} not existed." };
				await context.RespondAsync(response);

				return;
			}

		}
		else if (requested.Length > 1)
		{
			var existed = await _context.Users
				.AsNoTracking()
				.Select(x => x.Email)
				.ToListAsync();

			var diff = requested.Except(existed);

			if (diff.Any())
			{
				var response = new NotFound() { Message = $"Emails: {string.Join(", ", diff)} not existed." };
				await context.RespondAsync(response);

				return;
			}
		}

		await context.RespondAsync(new Existed());
	}
}
