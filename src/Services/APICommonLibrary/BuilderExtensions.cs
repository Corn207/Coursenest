using APICommonLibrary.Options;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace APICommonLibrary;
public static class BuilderExtensions
{
	public static void UsingConfiguredRabbitMq(this IBusRegistrationConfigurator busConfig, IConfiguration builderConfig)
	{
		busConfig.UsingRabbitMq((context, config) =>
		{
			config.Host(
				builderConfig[$"{ConnectionOptions.SectionName}:{nameof(ConnectionOptions.MessageBus)}"],
				"/",
				h => {
					h.Username("guest");
					h.Password("guest");
				});

			config.ConfigureEndpoints(context);
		});
	}
}
