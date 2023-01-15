using Authentication.API.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using TestCommonLibrary;

namespace Authentication.Tests;

[TestFixture]
public class RolesControllerTests
{
	private WebApplicationFactory<Program> _factory;
	private HttpClient _client;

	[OneTimeSetUp]
	public async Task Setup()
	{
		_factory = await new WebApplicationFactoryBuilder()
			.BuildAsync<Program>();
		await _factory.DatabaseInitializeAsync(Defaults.Database);

		_client = _factory.CreateClient();
	}


	[Test]
	[TestCase(2)]
	[TestCase(7)]
	public async Task GetAll_UserId_Returns200(int userId)
	{
		// Arrange

		// Act
		var response = await _client.GetAsync($"/roles?userId={userId}");

		// Assert
		Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
	}


	[Test]
	[TestCase(2)]
	[TestCase(7)]
	public async Task GetAllMe_ReturnsOk(int userId)
	{
		// Arrange
		var client = _factory.CreateClient();
		client.DefaultRequestHeaders.Add("userId", userId.ToString());

		// Act
		var response = await client.GetAsync("/roles/me");

		// Assert
		Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
	}


	[Test]
	public async Task Update_ReturnsExactInput()
	{
		// Arrange
		int userId = 1;
		var dto = new SetRole() { Type = API.Infrastructure.Entities.RoleType.Student, Expiry = DateTime.Now };

		// Act
		var response = await _client.PutAsJsonAsync($"/roles/{userId}", dto);
		response.EnsureSuccessStatusCode();
		var results = await _client.GetFromJsonAsync<IEnumerable<RoleResult>>($"/roles?userId={userId}");

		// Assert
		Assert.That(results, Is.Not.Null);
		Assert.Multiple(() =>
		{
			Assert.That(results.Any(x => x.Type == dto.Type), Is.True);
			Assert.That(results.Any(x => x.Expiry == dto.Expiry), Is.True);
		});
	}
}
