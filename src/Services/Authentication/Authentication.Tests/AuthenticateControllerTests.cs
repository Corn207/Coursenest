using APICommonLibrary.MessageBus.Commands;
using APICommonLibrary.MessageBus.Responses;
using Authentication.API.DTOs;
using MassTransit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using TestCommonLibrary;

namespace Authentication.Tests;

[TestFixture]
[NonParallelizable]
public class AuthenticateControllerTests
{
	private WebApplicationFactory<Program> _factory;
	private HttpClient _client;

	[OneTimeSetUp]
	public async Task Setup()
	{
		_factory = await new WebApplicationFactoryBuilder()
			.AddMassTransitTestHarness(x =>
			{
				x.AddHandler<CreateUser>(context =>
				{
					return context.RespondAsync(new Created() { Id = 8 });
				});

				x.AddHandler<CheckTopicIds>(context =>
				{
					return context.RespondAsync(new Existed());
				});

				x.AddHandler<CheckUserEmails>(context =>
				{
					return context.RespondAsync(new Existed());
				});
			})
			.BuildAsync<Program>();
		await _factory.DatabaseInitializeAsync(Defaults.Database);

		_client = _factory.CreateClient();
	}


	[Test]
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


	[Test]
	public async Task Login_ReturnsTokensResult()
	{
		// Arrange
		var login = new Login() { Username = "usrnonad", Password = "pwd" };

		// Act
		var loginContent = await _client
			.PostParsedAsync<Login, TokensResult>("authenticate/login", login);

		// Assert
		Assert.Pass();
	}


	[Test]
	public async Task Logout_ReturnsOk()
	{
		// Arrange
		var login = new Login() { Username = "usrnonad", Password = "pwd" };
		var client = _factory.CreateClient();

		// Act
		var loginContent = await _client
			.PostParsedAsync<Login, TokensResult>("authenticate/login", login);

		client.DefaultRequestHeaders.Add("userId", loginContent.UserId.ToString());
		var response = await client.PostAsync("/authenticate/logout", null);

		// Assert
		Assert.That(response.IsSuccessStatusCode);
	}


	[Test]
	public async Task Refresh_ReturnsCorrectModel()
	{
		// Arrange
		var login = new Login() { Username = "usrad", Password = "pwd" };

		// Act
		var loginContent = await _client
			.PostParsedAsync<Login, TokensResult>("authenticate/login", login);

		var refreshContent = await _client
			.PostParsedAsync<string, AccessTokenResult>("authenticate/refresh", loginContent.RefreshToken);

		// Assert
		Assert.Pass();
	}


	[Test]
	[TestCase(1)]
	[TestCase(2)]
	public async Task ResetPassword_ReturnsOk(int userId)
	{
		// Arrange
		var jsonContent = JsonContent.Create(userId);

		// Act
		var response = await _client.PutAsync("authenticate/reset-password", jsonContent);

		// Assert
		Assert.That(response.IsSuccessStatusCode, Is.True);
	}


	[Test]
	[TestCase("usrbasic")]
	[TestCase("usrstd")]
	public async Task ForgotPassword_ReturnsOk(string username)
	{
		// Arrange
		var content = new ForgotPassword() { Email = "test", Username = username };

		// Act
		var response = await _client.PutAsJsonAsync("authenticate/forgot-password", content);

		// Assert
		Assert.That(response.IsSuccessStatusCode, Is.True);
	}


	public static readonly object[] ChangePasswordData = new[]
	{
		new object[] { 3, new ChangePassword() { OldPassword = "pwd", NewPassword = "pwdUpdate" } },
		new object[] { 4, new ChangePassword() { OldPassword = "pwd", NewPassword = "pwdUpdate" } }
	};
	[Test]
	[TestCaseSource(nameof(ChangePasswordData))]
	public async Task ChangePassword_ReturnsOk(int userId, ChangePassword content)
	{
		// Arrange
		var client = _factory.CreateClient();
		client.DefaultRequestHeaders.Add(nameof(userId), userId.ToString());

		// Act
		var response = await client.PutAsJsonAsync("authenticate/change-password", content);

		// Assert
		Assert.That(response.IsSuccessStatusCode, Is.True);
	}
}
