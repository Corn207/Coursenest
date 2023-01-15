using APICommonLibrary.MessageBus.Commands;
using APICommonLibrary.MessageBus.Responses;
using Identity.API.DTOs;
using MassTransit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using TestCommonLibrary;

namespace Identity.Tests;

[TestFixture]
public class UsersControllerTests
{
	private WebApplicationFactory<Program> _factory;
	private HttpClient _client;

	[OneTimeSetUp]
	public async Task Setup()
	{
		_factory = await new WebApplicationFactoryBuilder()
			.AddMassTransitTestHarness(x =>
			{
				x.AddHandler<CheckTopics>(context => context.RespondAsync(new Existed()));
			})
			.BuildAsync<Program>();
		await _factory.DatabaseInitializeAsync(Defaults.Database);

		_client = _factory.CreateClient();
	}


	public static readonly UserQuery[] GetAllData = new[]
	{
		new UserQuery() { FullName = "Em" },
		new UserQuery() { Page = 1, PageSize = 3 },
		new UserQuery()
	};
	[Test]
	[TestCaseSource(nameof(GetAllData))]
	public async Task GetAll_UserQueries_ReturnsUserResults(UserQuery query)
	{
		// Arrange
		var queries = new List<KeyValuePair<string, string?>>
		{
			new KeyValuePair<string, string?>(nameof(UserQuery.Page), query.Page.ToString()),
			new KeyValuePair<string, string?>(nameof(UserQuery.PageSize), query.PageSize.ToString())
		};
		if (!string.IsNullOrWhiteSpace(query.FullName))
		{
			queries.Add(new KeyValuePair<string, string?>(nameof(UserQuery.FullName), query.FullName));
		}

		var queryString = QueryString.Create(queries);

		// Act
		var results = await _client.GetFromJsonAsync<IEnumerable<UserResult>>("/users" + queryString.ToString());

		// Assert
		Assert.That(results, Is.Not.Null);
	}


	[Test]
	public async Task GetCount_ReturnsSuccessStatusCode()
	{

		// Arrange

		// Act
		var response = await _client.GetAsync("/users/count");

		// Assert
		Assert.That(response.IsSuccessStatusCode);
	}


	[Test]
	[TestCase(2, "Mona")]
	[TestCase(3, "Emma")]
	public async Task Get_ReturnsExactFullname(int userId, string fullName)
	{
		// Arrange


		// Act
		var result = await _client.GetFromJsonAsync<UserResult>($"/users/{userId}");

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.FullName, Is.EqualTo(fullName));
	}


	[Test]
	[TestCase(1, "Hanoi")]
	[TestCase(2, "Ho Chi Minh City")]
	public async Task GetProfile_ReturnsExactLocation(int userId, string location)
	{
		// Arrange


		// Act
		var result = await _client.GetFromJsonAsync<UserProfileResult>($"/users/{userId}/profile");

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Location, Is.EqualTo(location));
	}


	[Test]
	[TestCase(1, "Developer")]
	[TestCase(2, "Stdent")]
	public async Task GetInstructor_ReturnsExactTitle(int userId, string title)
	{
		// Arrange


		// Act
		var result = await _client.GetFromJsonAsync<UserInstructorResult>($"/users/{userId}/instructor");

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Title, Is.EqualTo(title));
	}


	public static readonly object[] UpdateData = new[]
	{
		new object[] { 2, new UpdateUser() { FullName = "John Smith", Location = "Berlin" } }
	};
	[Test]
	[TestCaseSource(nameof(UpdateData))]
	public async Task Update_ReturnsExactlyInput(int userId, UpdateUser content)
	{
		// Arrange
		var values = content.GetType().GetProperties()
			.Select(pi => (pi.Name, pi.GetValue(content)))
			.Where(v => v.Item2 != null);

		var client = _factory.CreateClient();
		client.DefaultRequestHeaders.Add("userId", userId.ToString());

		// Act
		var response = await client.PutAsJsonAsync($"/users/me", content);
		response.EnsureSuccessStatusCode();
		var result = await client.GetFromJsonAsync<UserProfileResult>($"/users/{userId}/profile");

		// Assert
		Assert.That(result, Is.Not.Null);

		var isExactly = result.GetType().GetProperties()
			.Join(values, outer => outer.Name, inner => inner.Name, (pi, inner) => (inner.Item2!, pi.GetValue(result)))
			.All(x => x.Item1.Equals(x.Item2));
		Assert.That(isExactly, Is.True);
	}


	[Test]
	[TestCase(7)]
	public async Task Delete_ReturnsOk(int userId)
	{
		// Arrange


		// Act
		var response = await _client.DeleteAsync($"/users/{userId}");
		response.EnsureSuccessStatusCode();
		response = await _client.GetAsync($"/users/{userId}");

		// Assert
		Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
	}


	[Test]
	[TestCase(2, 4)]
	public async Task AddInterestedTopic_ReturnsExactlyInput(int userId, int topicId)
	{
		// Arrange
		var client = _factory.CreateClient();
		client.DefaultRequestHeaders.Add("userId", userId.ToString());

		// Act
		var response = await client.PostAsync($"/users/me/interest/{topicId}", null);
		response.EnsureSuccessStatusCode();
		var result = await client.GetFromJsonAsync<UserProfileResult>($"/users/{userId}/profile");

		// Assert
		Assert.That(result, Is.Not.Null);

		Assert.That(result.InterestedTopics, Does.Contain(topicId));
	}


	[Test]
	[TestCase(2, 3)]
	public async Task DeleteInterestedTopic_ReturnsExactlyInput(int userId, int topicId)
	{
		// Arrange
		var client = _factory.CreateClient();
		client.DefaultRequestHeaders.Add("userId", userId.ToString());

		// Act
		var response = await client.DeleteAsync($"/users/me/interest/{topicId}");
		response.EnsureSuccessStatusCode();
		var result = await client.GetFromJsonAsync<UserProfileResult>($"/users/{userId}/profile");

		// Assert
		Assert.That(result, Is.Not.Null);

		Assert.That(result.InterestedTopics, Does.Not.Contain(topicId));
	}


	[Test]
	[TestCase(2)]
	[TestCase(3)]
	public async Task GetAllFollowedTopic_ReturnsOk(int userId)
	{
		// Arrange
		var client = _factory.CreateClient();
		client.DefaultRequestHeaders.Add("userId", userId.ToString());

		// Act
		var result = await client.GetFromJsonAsync<IEnumerable<int>>($"/users/me/follow");

		// Assert
		Assert.That(result, Is.Not.Null);
	}


	[Test]
	[TestCase(3, 1)]
	public async Task AddFollowedTopic_Returns201(int userId, int topicId)
	{
		// Arrange
		var client = _factory.CreateClient();
		client.DefaultRequestHeaders.Add("userId", userId.ToString());

		// Act
		var response = await client.PostAsync($"/users/me/follow/{topicId}", null);

		// Assert
		Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
	}


	[Test]
	[TestCase(3, 2)]
	public async Task DeleteFollowedTopic_ReturnsExactlyInput(int userId, int topicId)
	{
		// Arrange
		var client = _factory.CreateClient();
		client.DefaultRequestHeaders.Add("userId", userId.ToString());

		// Act
		var response = await client.DeleteAsync($"/users/me/follow/{topicId}");

		// Assert
		Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
	}


	[Test]
	public async Task AddExperience_ReturnsExactInput()
	{
		// Arrange
		var userId = 2;
		var client = _factory.CreateClient();
		client.DefaultRequestHeaders.Add("userId", userId.ToString());

		var content = new CreateExperience()
		{
			Name = "School A",
			Title = "Engineer",
			Started = DateTime.Now.AddYears(-1),
			Ended = DateTime.Now
		};

		// Act
		var response = await client.PostAsJsonAsync($"/users/me/experiences", content);
		response.EnsureSuccessStatusCode();
		var result = await client.GetFromJsonAsync<UserProfileResult>($"/users/{userId}/profile");

		// Assert
		Assert.That(result, Is.Not.Null);

		var actual = result.Experiences
			.Any(x =>
				x.Name == content.Name &&
				x.Title == content.Title &&
				x.Started == content.Started &&
				x.Ended == content.Ended);
		Assert.That(actual, Is.True);
	}

	[Test]
	[TestCase(2, 2)]
	public async Task DeleteExperience_ReturnsExactInput(int userId, int experienceId)
	{
		// Arrange
		var client = _factory.CreateClient();
		client.DefaultRequestHeaders.Add("userId", userId.ToString());

		// Act
		var response = await client.DeleteAsync($"/users/me/experiences/{experienceId}");
		response.EnsureSuccessStatusCode();
		var result = await client.GetFromJsonAsync<UserProfileResult>($"/users/{userId}/profile");

		// Assert
		Assert.That(result, Is.Not.Null);

		var expected = Is.False;
		var actual = result.Experiences.Any(x => x.ExperienceId == experienceId);
		Assert.That(actual, expected);
	}
}
