namespace APICommonLibrary.MessageBus.Commands;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record GetTopicResult
{
	public int TopicId { get; set; }
	public string Content { get; set; }
}
