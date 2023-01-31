using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Responses;
using Identity.API.Consumers;
using Identity.API.Infrastructure.Contexts;
using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using TestCommonLibrary;

namespace Identity.Tests;

[TestFixture]
[NonParallelizable]
public class ConsumerTests
{
	private WebApplicationFactory<Program> _factory;
	private ITestHarness _harness;

	[OneTimeSetUp]
	public async Task Setup()
	{
		_factory = await new WebApplicationFactoryBuilder()
			.AddMassTransitTestHarness(x =>
			{
				x.AddConsumer<CheckUserEmailsConsumer>();
				x.AddConsumer<CreateUserAchievementConsumer>();
				x.AddConsumer<CreateUserConsumer>();
			})
			.BuildAsync<Program>();
		await _factory.DatabaseInitializeAsync(Defaults.Database);

		_harness = _factory.Services.GetTestHarness();
	}


	[Test]
	public async Task CheckUserEmailsConsumer_ReturnsExisted()
	{
		// Arrange
		var client = _harness.GetRequestClient<CheckUserEmails>();
		var request = new CheckUserEmails()
		{
			Emails = new[] { "one@gmail.com", "three@gmail.com" }
		};

		// Act
		var response = await client.GetResponse<Existed, NotFound>(request);

		// Assert
		Assert.That(response.Is(out Response<Existed>? _), Is.True);
	}


	[Test]
	public async Task CreateUserAchievementConsumer_ReturnsCreatedResource()
	{
		// Arrange
		var client = _harness.GetRequestClient<CreateUserAchievement>();
		var request = new CreateUserAchievement()
		{
			Created = DateTime.Now,
			Title = "Learned CSS.",
			UserId = 1
		};

		using var scope = _factory.Services.CreateScope();
		var context = scope.ServiceProvider.GetRequiredService<DataContext>();

		// Act
		var response = await client.GetResponse<Created, NotFound>(request);
		if (response.Is(out Response<NotFound>? notFountResponse))
		{
			throw new Exception(notFountResponse!.Message.Message);
		}

		var exists = await context.Achievements
			.AsNoTracking()
			.AnyAsync(x =>
				x.Created == request.Created &&
				x.Title == request.Title &&
				x.UserId == request.UserId);

		// Assert
		Assert.That(exists, Is.True);
	}


	[Test]
	public async Task CreateUserConsumer_ReturnsCreatedResource()
	{
		// Arrange
		var client = _harness.GetRequestClient<CreateUser>();
		var request = new CreateUser()
		{
			Email = "testing@test.com",
			FullName = "Tester A",
			InterestedTopicIds = new[] { 1, 2 }
		};

		using var scope = _factory.Services.CreateScope();
		var context = scope.ServiceProvider.GetRequiredService<DataContext>();

		// Act
		var response = await client.GetResponse<Created, Existed>(request);
		if (response.Is(out Response<Existed>? existedResponse))
		{
			throw new Exception(existedResponse!.Message.Message);
		}

		var exists = await context.Users
			.AsNoTracking()
			.AnyAsync(x =>
				x.Email == request.Email &&
				x.FullName == request.FullName &&
				x.InterestedTopics.Count == request.InterestedTopicIds.Length);

		// Assert
		Assert.That(exists, Is.True);
	}
}
