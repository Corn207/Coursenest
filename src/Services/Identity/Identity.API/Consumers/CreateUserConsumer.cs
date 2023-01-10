using APICommonLibrary.MessageBus.Commands;
using AutoMapper;
using Identity.API.Infrastructure.Contexts;
using Identity.API.Infrastructure.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Consumers;

public class CreateUserConsumer : IConsumer<CreateUser>
{
	private readonly IMapper _mapper;
	private readonly DataContext _context;

	public CreateUserConsumer(IMapper mapper, DataContext context)
	{
		_mapper = mapper;
		_context = context;
	}

	public async Task Consume(ConsumeContext<CreateUser> context)
	{
		var exist = await _context.Users.AnyAsync(x => x.Email == context.Message.Email);
		if (exist) throw new ArgumentException("Email existed.");

		var user = _mapper.Map<User>(context.Message);

		_context.Users.Add(user);
		await _context.SaveChangesAsync();

		var result = new CreateUserResult() { UserId = user.UserId };

		await context.RespondAsync(result);
	}
}
