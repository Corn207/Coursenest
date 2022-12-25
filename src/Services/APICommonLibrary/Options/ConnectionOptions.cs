using System.ComponentModel.DataAnnotations;

namespace APICommonLibrary.Options;
public class ConnectionOptions
{
	public const string SectionName = "Connections";

	public string Database { get; set; } = string.Empty;

	public string MessageBus { get; set; } = string.Empty;
}
