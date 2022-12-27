namespace APICommonLibrary.Options;

#nullable disable

public class DatabaseOptions
{
	public bool Overwrite { get; set; } = false;
	public bool Create { get; set; } = false;
	public bool Seed { get; set; } = false;
}
