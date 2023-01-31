using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Responses;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Payment.API.DTOs;
using System.Globalization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Payment.API.Controllers;
[Route("[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
	private readonly IRequestClient<ExtendRole> _setRoleClient;

	public PaymentController(
		IRequestClient<ExtendRole> setRoleClient)
	{
		_setRoleClient = setRoleClient;
	}

	// POST: /payment
	[HttpPost]
	public async Task<ActionResult> Pay(
		[FromHeader] int userId,
		[FromBody] Pay body)
	{
		var cardExpiry = DateTime.ParseExact(
			$"{body.ExpiryDateMonth}/{body.ExpiryDateYear}",
			"MM/yy",
			CultureInfo.InvariantCulture);
		if (cardExpiry < DateTime.Now)
		{
			return BadRequest("Card expired.");
		}

		var request = new ExtendRole()
		{
			UserId = userId,
			Type = body.Role,
			ExtendedDays = body.Months * 30
		};
		var response = await _setRoleClient.GetResponse<Succeeded, NotFound>(request);

		if (response.Is(out Response<Succeeded> _))
		{
			return Ok();
		}
		else
		{
			response.Is(out Response<NotFound>? notFoundResponse);
			return NotFound(notFoundResponse!.Message.Message);
		}
	}
}
