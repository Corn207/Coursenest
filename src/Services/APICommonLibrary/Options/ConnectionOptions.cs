using System.ComponentModel.DataAnnotations;

namespace APICommonLibrary.Options;

#nullable disable

public class ConnectionOptions
{
	[Required]
	public string Database { get; set; }
	[Required]
	public string MessageBus { get; set; }
}
