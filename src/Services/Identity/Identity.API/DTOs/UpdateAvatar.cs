using APICommonLibrary.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Identity.API.DTOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record UpdateAvatar
{
	[FromForm]
	[BindRequired]
	[MaxSize(0, 2 * 1024 * 1024)]
	[ImageExtension]
	public IFormFile FormFile { get; set; }

	[FromHeader]
	public int UserId { get; set; }
}
