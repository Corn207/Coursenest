using APICommonLibrary.MessageBus.Commands;
using APICommonLibrary.MessageBus.Responses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Library.API.Infrastructure.Contexts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Consumers;

public class GetExamConsumer : IConsumer<GetExam>
{
	private readonly IMapper _mapper;
	private readonly DataContext _context;

	public GetExamConsumer(IMapper mapper, DataContext context)
	{
		_mapper = mapper;
		_context = context;
	}

	public async Task Consume(ConsumeContext<GetExam> context)
	{
		var result = await _context.Exams
			.Include(x => x.Questions)
			.ThenInclude(x => x.Choices)
			.Where(x => x.UnitId == context.Message.UnitId && x.Lesson.Course.IsApproved)
			.ProjectTo<ExamResult>(_mapper.ConfigurationProvider)
			.SingleOrDefaultAsync();
		if (result == null)
		{
			var response = new NotFound() { Message = $"Approved Exam does not exist." };
			await context.RespondAsync(response);

			return;
		}

		await context.RespondAsync(result);
	}
}
