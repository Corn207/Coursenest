using APICommonLibrary.MessageBus.Commands;
using Identity.API.DTOs;
using Identity.API.Infrastructure.Contexts;
using MassTransit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
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
			.AddSqliteInMemory<DataContext>()
			.AddMassTransitTestHarness(x =>
			{
				x.AddHandler<GetTopic>(context =>
				{
					return context.RespondAsync(new GetTopicResult()
					{
						TopicId = 1,
						Content = "Javascript"
					});
				});
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
		var response = await _client.GetAsync("/users" + queryString.ToString());
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var results = JsonConvert.DeserializeObject<IEnumerable<UserResult>>(body);

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


	public static readonly object[] GetData = new[]
	{
		new object[] { 2, "Mona" },
		new object[] { 3, "Emma" }
	};
	[Test]
	[TestCaseSource(nameof(GetData))]
	public async Task Get_ReturnsExactFullname(int userId, string fullName)
	{
		// Arrange


		// Act
		var response = await _client.GetAsync($"/users/{userId}");
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var result = JsonConvert.DeserializeObject<UserResult>(body);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.FullName, Is.EqualTo(fullName));
	}


	public static readonly object[] GetProfileData = new[]
	{
		new object[] { 1, "Hanoi" },
		new object[] { 2, "Ho Chi Minh City" }
	};
	[Test]
	[TestCaseSource(nameof(GetProfileData))]
	public async Task GetProfile_ReturnsExactLocation(int userId, string location)
	{
		// Arrange


		// Act
		var response = await _client.GetAsync($"/users/{userId}/profile");
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var result = JsonConvert.DeserializeObject<UserProfileResult>(body);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Location, Is.EqualTo(location));
	}


	public static readonly object[] GetInstructorData = new[]
	{
		new object[] { 1, "Developer" },
		new object[] { 2, "Stdent" }
	};
	[Test]
	[TestCaseSource(nameof(GetInstructorData))]
	public async Task GetInstructor_ReturnsExactTitle(int userId, string title)
	{
		// Arrange


		// Act
		var response = await _client.GetAsync($"/users/{userId}/instructor");
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var result = JsonConvert.DeserializeObject<UserInstructorResult>(body);

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
	public async Task Update_ReturnsExactlyInput(int userId, UpdateUser dto)
	{
		// Arrange
		var values = dto.GetType().GetProperties()
			.Select(pi => (pi.Name, pi.GetValue(dto)))
			.Where(v => v.Item2 != null);

		var client = _factory.CreateClient();
		client.DefaultRequestHeaders.Add("userId", userId.ToString());
		var content = JsonContent.Create(dto);

		// Act
		var response = await client.PutAsync($"/users/me", content);
		response.EnsureSuccessStatusCode();
		response = await client.GetAsync($"/users/{userId}/profile");
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var result = JsonConvert.DeserializeObject<UserProfileResult>(body);

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
		response = await client.GetAsync($"/users/{userId}/profile");
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var result = JsonConvert.DeserializeObject<UserProfileResult>(body);

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
		response = await client.GetAsync($"/users/{userId}/profile");
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var result = JsonConvert.DeserializeObject<UserProfileResult>(body);

		// Assert
		Assert.That(result, Is.Not.Null);

		Assert.That(result.InterestedTopics, Does.Not.Contain(topicId));
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


	public static readonly object[] AddExperiencesData = new[]
	{
		new object[]
		{
			2,
			new CreateExperience()
			{
				Name = "School A",
				Title = "Engineer",
				Started = DateTime.Now.AddYears(-1),
				Ended = DateTime.Now
			}
		}
	};
	[Test]
	[TestCaseSource(nameof(AddExperiencesData))]
	public async Task AddExperience_ReturnsExactInput(int userId, CreateExperience dto)
	{
		// Arrange
		var client = _factory.CreateClient();
		client.DefaultRequestHeaders.Add("userId", userId.ToString());
		var content = JsonContent.Create(dto);

		// Act
		var response = await client.PostAsync($"/users/me/experiences", content);
		response.EnsureSuccessStatusCode();
		response = await client.GetAsync($"/users/{userId}/profile");
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var result = JsonConvert.DeserializeObject<UserProfileResult>(body);

		// Assert
		Assert.That(result, Is.Not.Null);

		var expected = Is.True;
		var actual = result.Experiences
			.Any(x =>
				x.Name == dto.Name &&
				x.Title == dto.Title &&
				x.Started == dto.Started &&
				x.Ended == dto.Ended);
		Assert.That(actual, expected);
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
		response = await client.GetAsync($"/users/{userId}/profile");
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var result = JsonConvert.DeserializeObject<UserProfileResult>(body);

		// Assert
		Assert.That(result, Is.Not.Null);

		var expected = Is.False;
		var actual = result.Experiences.Any(x => x.ExperienceId == experienceId);
		Assert.That(actual, expected);
	}
}
