namespace APICommonLibrary.Constants;
public static class FormFileContants
{
	public static IReadOnlyDictionary<string, string> Extensions
		=> new Dictionary<string, string>()
		{
			{ ".jpg", "image/jpeg" },
			{ ".jpeg", "image/jpeg" },
			{ ".jfif", "image/jpeg" },
			{ ".pjpeg", "image/jpeg" },
			{ ".pjp", "image/jpeg" },
			{ ".png", "image/png" },
			{ ".webp", "image/webp" }
		};
}
