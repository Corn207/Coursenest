namespace APICommonLibrary.Options;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class DatabaseOptions
{
	public string ConnectionString { get; set; }
	public bool EnsureDeleted { get; set; }
	public bool EnsureCreated { get; set; }
}
