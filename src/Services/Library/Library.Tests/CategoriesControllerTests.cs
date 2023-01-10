using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace Library.Tests;

public class CategoriesControllerTests
{
	private readonly DefaultWebApplicationFactory _factory;
	private readonly HttpClient _client;

	public CategoriesControllerTests()
	{
		_factory = new DefaultWebApplicationFactory();
		_client = _factory.CreateClient();
	}

	[SetUp]
	public void Setup()
	{

	}

	[Test]
	public async Task Get_GetAll_ReturnsOk()
	{
		var message = await _client.GetAsync("/categories");

		Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.OK));
	}
}