using System.ComponentModel.DataAnnotations;

namespace UserData.API.DTOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record GradingSubmission
{
	public IEnumerable<Criterion> Criteria { get; set; }


	public class Criterion : IValidatableObject
	{
		public string Content { get; set; }
		public List<Checkpoint> Checkpoints { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (Checkpoints.Count(x => x.IsChosen) != 1)
			{
				yield return new ValidationResult(
					$"Criterion ({Content}) must have exactly 1 chosen Checkpoint.");
			}
		}
	}

	public class Checkpoint
	{
		public string Content { get; set; }

		[Range(0, int.MaxValue)]
		public int Point { get; set; }
		public bool IsChosen { get; set; }
	}
}
