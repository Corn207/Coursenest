namespace Identity.API.DTOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record UserQuery
{
	public string? FullName { get; set; }
	public int Page { get; set; } = 0;
	public int PageSize { get; set; } = 5;
}
