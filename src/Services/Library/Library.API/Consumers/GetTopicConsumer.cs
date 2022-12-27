using APICommonLibrary.MessageBus.Commands;
using Library.API.Infrastructure.Contexts;
using MassTransit;

namespace Library.API.Consumers;

public class GetTopicConsumer : IConsumer<GetTopic>
{
	private readonly DataContext _context;

	public GetTopicConsumer(DataContext context)
	{
		_context = context;
	}

	public async Task Consume(ConsumeContext<GetTopic> context)
	{
		var topic = await _context.Topics.FindAsync(context.Message.TopicId);
		if (topic == null) throw new KeyNotFoundException("Topic not existed.");

		await context.RespondAsync(new GetTopicResult()
		{
			TopicId = topic.TopicId,
			Content = topic.Content
		});
	}
}
