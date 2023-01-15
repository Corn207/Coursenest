using APICommonLibrary.MessageBus.Commands;
using APICommonLibrary.MessageBus.Events;
using APICommonLibrary.MessageBus.Responses;
using Authentication.API.Consumers;
using Authentication.API.Infrastructure.Contexts;
using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using TestCommonLibrary;
using RoleType = APICommonLibrary.MessageBus.Commands.RoleType;

namespace Authentication.Tests;

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
				x.AddConsumer<ExtendRoleConsumer>();
				x.AddConsumer<UserDeletedConsumer>();
			})
			.BuildAsync<Program>();
		await _factory.DatabaseInitializeAsync(Defaults.Database);

		_harness = _factory.Services.GetTestHarness();
	}


	[Test]
	public async Task ExtendRoleConsumer_ReturnsSucceeded()
	{
		// Arrange
		var client = _harness.GetRequestClient<ExtendRole>();
		var request = new ExtendRole()
		{
			UserId = 1,
			Type = RoleType.Student,
			ExtendedDays = 30
		};

		using var scope = _factory.Services.CreateScope();
		var context = scope.ServiceProvider.GetRequiredService<DataContext>();

		// Act
		var response = await client.GetResponse<Succeeded, NotFound>(request);
		if (response.Is(out Response<NotFound> _))
		{
			throw new Exception("Not Found");
		}

		var exists = await context.Roles
			.AsNoTracking()
			.AnyAsync(x =>
				x.CredentialUserId == request.UserId &&
				x.Expiry.Date == DateTime.Now.Date.AddDays(request.ExtendedDays));

		// Assert
		Assert.That(exists, Is.True);
	}


	[Test]
	public async Task UserDeletedConsumer_ReturnsNotExisted()
	{
		// Arrange
		var client = _harness.GetRequestClient<ExtendRole>();
		var request = new UserDeleted()
		{
			UserId = 2
		};

		using var scope = _factory.Services.CreateScope();
		var context = scope.ServiceProvider.GetRequiredService<DataContext>();

		var endpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

		// Act
		await endpoint.Publish(request);
		var list = await context.Credentials
			.AsNoTracking()
			.ToListAsync();

		var consumed = await _harness.Consumed.Any<ExtendRole>();

		var exists = await context.Credentials
			.AsNoTracking()
			.AnyAsync(x => x.UserId == request.UserId);

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(consumed, Is.True);
			Assert.That(exists, Is.False);
		});
	}
}
