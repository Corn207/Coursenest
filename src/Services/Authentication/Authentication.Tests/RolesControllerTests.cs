using Authentication.API.Infrastructure.Contexts;
using Microsoft.AspNetCore.Mvc.Testing;
using TestCommonLibrary;

namespace Authentication.Tests;

[TestFixture]
public class RolesControllerTests
{
	private WebApplicationFactory<Program> _factory;

	[OneTimeSetUp]
	public async Task Setup()
	{
		_factory = await new WebApplicationFactoryBuilder()
			.AddSqliteInMemory<DataContext>()
			.BuildAsync<Program>();

		await _factory.DatabaseInitializeAsync(Defaults.Database);
	}


	[Test]
	[TestCase(2)]
	[TestCase(7)]
	public async Task GetAll_UserId_ReturnsOk(int userId)
	{
		// Arrange
		var client = _factory.CreateClient();

		// Act
		var response = await client.GetAsync($"/roles?userId={userId}");

		// Assert
		Assert.That(response.IsSuccessStatusCode);
	}


	[Test]
	[TestCase(2)]
	[TestCase(7)]
	public async Task GetAllMe_UserId_ReturnsOk(int userId)
	{
		// Arrange
		var client = _factory.CreateClient();
		client.DefaultRequestHeaders.Add("userId", userId.ToString());

		// Act
		var response = await client.GetAsync("/roles/me");

		// Assert
		Assert.That(response.IsSuccessStatusCode);
	}
}
