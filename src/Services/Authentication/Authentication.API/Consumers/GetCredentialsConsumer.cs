using Authentication.API.Infrastructure.Contexts;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Responses;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Authentication.API.Consumers;
public class GetCredentialsConsumer : IConsumer<GetCredentials>
{
	private readonly IMapper _mapper;
	private readonly DataContext _context;

	public GetCredentialsConsumer(IMapper mapper, DataContext context)
	{
		_mapper = mapper;
		_context = context;
	}

	public async Task Consume(ConsumeContext<GetCredentials> context)
	{
		var credentials = await _context.Credentials
			.ProjectTo<CredentialsResult.Credential>(_mapper.ConfigurationProvider)
			.Where(x => context.Message.Ids.Contains(x.UserId))
			.ToListAsync();

		var response = new CredentialsResult()
		{
			Credentials = credentials
		};
		await context.RespondAsync(response);
	}
}
