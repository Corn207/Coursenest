using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Responses;
using Identity.API.Infrastructure.Contexts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Consumers;

public class CheckUsersConsumer : IConsumer<CheckUsers>
{
	private readonly DataContext _context;

	public CheckUsersConsumer(DataContext context)
	{
		_context = context;
	}

	public async Task Consume(ConsumeContext<CheckUsers> context)
	{
		var validQueries = context.Message.Queries
			.Where(q => 
				(q.Id == null || q.Id > 0) &&
				(q.Email == null || string.IsNullOrWhiteSpace(q.Email)) &&
				!(q.Id == null && q.Email == null))
			.Distinct();

		var fullQueries = validQueries
			.Where(q => q.Id != null && q.Email != null);

		var idOnlyQueries = validQueries
			.Where(q => q.Id != null && q.Email == null);

		var emailOnlyQueries = validQueries
			.Where(q => q.Id == null && q.Email != null);

		var extendIdQueries = idOnlyQueries
			.ExceptBy(fullQueries.Select(x => x.Id), x => x.Id);

		var extendEmailQueries = emailOnlyQueries
			.ExceptBy(fullQueries.Select(x => x.Email), x => x.Email);

		var queries = fullQueries
			.Concat(extendIdQueries)
			.Concat(extendEmailQueries);

		var users = await _context.Users
			.Select(x => new { x.UserId, x.Email })
			.Where(x => queries
				.Any(q => 
					(q.Id == null || q.Id == x.UserId) &&
					(q.Email == null || q.Id == x.UserId)))
			.ToListAsync();

		var missingId = queries
			.Where(x => x.Id != null)
			.Select(x => (int)x.Id!)
			.Except(users.Select(x => x.UserId));

		var missingEmail = queries
			.Where(x => x.Email != null)
			.Select(x => x.Email)
			.Except(users.Select(x => x.Email));

		var response = new NotFound();
		if (missingId.Any())
		{
			response.Message += $"Ids: {string.Join(", ", missingId)} do not exist.";
		}
		if (missingEmail.Any())
		{
			response.Message += $"Emails: {string.Join(", ", missingEmail)} do not exist.";
		}
		if (response.Message == null)
		{
			await context.RespondAsync(response);
		}
		else
		{
			await context.RespondAsync(new Existed());
		}
	}
}
