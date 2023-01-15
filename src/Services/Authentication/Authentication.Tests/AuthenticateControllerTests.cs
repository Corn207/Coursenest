using APICommonLibrary.MessageBus.Commands;
using Authentication.API.DTOs;
using MassTransit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;
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
			.AddMassTransitTestHarness(x =>
			{
				x.AddHandler<CreateUser>(context =>
				{
					return context.RespondAsync(new CreateUserResult()
					{
						UserId = 8
					});
				});

				x.AddHandler<GetTopic>(context =>
				{
					return context.RespondAsync(new GetTopicResult()
					{
						TopicId = 8,
						Content = "Test Topic"
					});
				});
			})
			.BuildAsync<Program>();
		await _factory.DatabaseInitializeAsync(Defaults.Database);

		_client = _factory.CreateClient();
	}


	[Test, Order(1)]
	public async Task Register_Returns201()
	{
		// Arrange
		var content = JsonContent.Create(new Register()
		{
			Username = "usrtest",
			Password = "pwdtest",
			Email = "testuser@test.com",
			Fullname = "Test Smith",
			InterestedTopicIds = new[] { 1 }
		});

		// Act
		var response = await _client.PostAsync("/authenticate/register", content);

		// Assert
		Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
	}


	[Test, Order(2)]
	public async Task Login_ReturnsTokensResult()
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
		var body = await response.Content.ReadAsStringAsync();
		var result = JsonSerializer.Deserialize<TokensResult>(body);

		// Assert
		Assert.That(result, Is.Not.Null);

		_refreshToken = result.RefreshToken;
	}


	[Test, Order(4)]
	public async Task Logout_ReturnsOk()
	{
		// Arrange
		var client = _factory.CreateClient();
		client.DefaultRequestHeaders.Add("UserId", "8");

		// Act
		var response = await client.PostAsync("/authenticate/logout", null);

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
		var body = await response.Content.ReadAsStringAsync();
		var result = JsonSerializer.Deserialize<AccessTokenResult>(body);

		// Assert
		Assert.That(result, Is.Not.Null);
	}
}
