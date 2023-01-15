using System.Net.Http.Json;

namespace TestCommonLibrary;
public static class HttpClientExtensions
{
	public static async Task<TResponse> PostParsedAsync<TRequest, TResponse>(
		this HttpClient client,
		string? requestUri,
		TRequest content)
	{
		var response = await client.PostAsJsonAsync(requestUri, content);
		response.EnsureSuccessStatusCode();
		var result = await response.Content.ReadFromJsonAsync<TResponse>();
		if (result == null) throw new ArgumentException($"Cannot parse as {nameof(TResponse)}");

		return result;
	}

	public static async Task<TResponse> PutParsedAsync<TRequest, TResponse>(
		this HttpClient client,
		string? requestUri,
		TRequest content)
	{
		var response = await client.PutAsJsonAsync(requestUri, content);
		response.EnsureSuccessStatusCode();
		var result = await response.Content.ReadFromJsonAsync<TResponse>();
		if (result == null) throw new ArgumentException($"Cannot parse as {nameof(TResponse)}");

		return result;
	}
}
