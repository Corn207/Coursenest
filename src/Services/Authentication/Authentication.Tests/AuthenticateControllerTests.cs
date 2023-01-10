using APICommonLibrary.MessageBus.Commands;
using Authentication.API.DTOs;
using Authentication.API.Infrastructure.Contexts;
using MassTransit;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using TestCommonLibrary;

namespace Authentication.Tests;

[TestFixture]
[NonParallelizable]
public class AuthenticateControllerTests
{
	private WebApplicationFactory<Program> _factory;
	private HttpClient _client;

	private string? _refreshToken;

	[OneTimeSetUp]
	public async Task Setup()
	{
		_factory = await new WebApplicationFactoryBuilder()
			.AddSqliteInMemory<DataContext>()
			.AddMassTransitTestHarness(x =>
			{
				x.AddHandler<CreateUser>(context =>
				{
					return context.RespondAsync(new CreateUserResult()
					{
						UserId = 1
					});
				});
			})
			.BuildAsync<Program>();
		_factory.DatabaseInitialize();

		_client = _factory.CreateClient();
	}


	[Test, Order(1)]
	public async Task Post_Register_ReturnsOk()
	{
		// Act
		var response = await _client.PostAsync("/authenticate/register", JsonContent.Create(new Register()
		{
			Username = "usrtest",
			Password = "pwdtest",
			Email = "testuser@test.com",
			Fullname = "Test Smith",
			InterestedTopicIds = new[] { 1 }
		}));

		// Assert
		Assert.That(response.IsSuccessStatusCode);
	}

	[Test, Order(2)]
	public async Task Post_Login_ReturnsTokensResult()
	{
		// Arrange
		var content = JsonContent.Create(new Login()
		{
			Username = "usrtest",
			Password = "pwdtest"
		});

		// Act
		var response = await _client.PostAsync("/authenticate/login", content);
		response.EnsureSuccessStatusCode();
		var responseString = await response.Content.ReadAsStringAsync();
		var result = JsonConvert.DeserializeObject<TokensResult>(responseString);

		// Assert
		Assert.That(result, Is.Not.Null);

		_refreshToken = result.RefreshToken;
	}

	[Test, Order(4)]
	public async Task Post_Logout_ReturnsOk()
	{
		// Arrange
		_client.DefaultRequestHeaders.Add("UserId", "1");

		// Act
		var response = await _client.PostAsync("/authenticate/logout", null);

		// Assert
		Assert.That(response.IsSuccessStatusCode);
	}

	[Test, Order(3)]
	public async Task Post_Refresh_ReturnsAccessTokenResult()
	{
		// Arrange
		var content = JsonContent.Create(_refreshToken);

		// Act
		var response = await _client.PostAsync("/authenticate/refresh", content);
		response.EnsureSuccessStatusCode();
		var responseString = await response.Content.ReadAsStringAsync();
		var result = JsonConvert.DeserializeObject<AccessTokenResult>(responseString);

		// Assert
		Assert.That(result, Is.Not.Null);
	}
}
