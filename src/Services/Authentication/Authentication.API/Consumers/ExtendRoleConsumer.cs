using APICommonLibrary.MessageBus.Commands;
using APICommonLibrary.MessageBus.Responses;
using Authentication.API.Infrastructure.Contexts;
using Authentication.API.Infrastructure.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Authentication.API.Consumers;
public class ExtendRoleConsumer : IConsumer<ExtendRole>
{
	private readonly DataContext _context;

	public ExtendRoleConsumer(DataContext context)
	{
		_context = context;
	}

	public async Task Consume(ConsumeContext<ExtendRole> context)
	{
		var credential = await _context.Credentials
			.Include(x => x.Roles)
			.FirstOrDefaultAsync(x => x.UserId == context.Message.UserId);
		if (credential == null)
		{
			var response = new NotFound() { Message = $"UserId: {context.Message.UserId} does not exist." };
			await context.RespondAsync(response);

			return;
		}

		var role = credential.Roles
			.FirstOrDefault(x => x.Type.ToString() == context.Message.Type.ToString());
		if (role == null)
		{
			role = new Role()
			{
				CredentialUserId = context.Message.UserId,
				Type = (APICommonLibrary.Models.Role)context.Message.Type,
				Expiry = DateTime.Now.AddDays(context.Message.ExtendedDays)
			};
			_context.Roles.Add(role);
		}
		else
		{
			role.Expiry = role.Expiry.AddDays(context.Message.ExtendedDays);
			_context.Roles.Update(role);
		}

		await _context.SaveChangesAsync();

		await context.RespondAsync(new Succeeded());
	}
}
