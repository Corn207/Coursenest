using APICommonLibrary.Contracts;
using Identity.API.Contexts;
using Identity.API.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Consumers;

public class AddUserRequestConsumer : IConsumer<AddUserRequest>
{
	private readonly DataContext _context;

	public AddUserRequestConsumer(DataContext context)
	{
		_context = context;
	}

	public async Task Consume(ConsumeContext<AddUserRequest> context)
	{
		var isExists = await _context.Users.AnyAsync(x => x.Email == context.Message.Email);
		if (isExists) throw new InvalidOperationException("Email existed.");

		var user = new User(context.Message.Email, context.Message.Fullname)
		{
			Created = DateTime.Now
		};
		_context.Users.Add(user);
		await _context.SaveChangesAsync();

		await context.RespondAsync(new AddUserResponse(user.UserId));
	}
}
