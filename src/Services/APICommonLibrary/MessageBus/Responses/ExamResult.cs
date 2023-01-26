namespace APICommonLibrary.MessageBus.Responses;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record ExamResult
{
	public string Title { get; set; }
	public string LessonTitle { get; set; }
	public string CourseTitle { get; set; }
	public TimeSpan TimeLimit { get; set; }
	public int UnitId { get; set; }
	public int? TopicId { get; set; }
	public List<Question> Questions { get; set; }

	public record Question
	{
		public string Content { get; set; }
		public byte Point { get; set; }
		public List<Choice> Choices { get; set; }
	}

	public record Choice
	{
		public string Content { get; set; }
		public bool IsCorrect { get; set; }
	}
}
