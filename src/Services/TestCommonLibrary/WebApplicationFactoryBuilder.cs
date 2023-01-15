using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace TestCommonLibrary;
public class WebApplicationFactoryBuilder
{
	private Action<IServiceCollection>? _massTransitAction;


	public WebApplicationFactoryBuilder AddMassTransitTestHarness(Action<IBusRegistrationConfigurator> config)
	{
		_massTransitAction = services =>
		{
			services.AddMassTransitTestHarness(config);
		};

		return this;
	}


	public async Task<WebApplicationFactory<TEntryPoint>> BuildAsync<TEntryPoint>() where TEntryPoint : class
	{
		var factory = new WebApplicationFactory<TEntryPoint>()
			.WithWebHostBuilder(config =>
			{
				config.ConfigureServices(services =>
				{
					_massTransitAction?.Invoke(services);
				});

				config.UseEnvironment("Development");
			});

		if (_massTransitAction != null)
		{
			var harness = factory.Services.GetTestHarness();
			await harness.Start();
		}

		return factory;
	}
}
