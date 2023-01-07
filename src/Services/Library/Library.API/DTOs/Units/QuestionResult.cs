using System.ComponentModel.DataAnnotations;

namespace Library.API.DTOs.Units;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record QuestionResult : IValidatableObject
{
	public string Content { get; set; }
	public int Point { get; set; }
	public List<ChoiceResult> Choices { get; set; }

	public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
	{
		var correctChoices = Choices.Where(x => x.IsCorrect);
		if (correctChoices.Count() != 1)
			yield return new ValidationResult($"Question: \"{Content}\" doesn't have exactly one correct choice.");
	}
}
