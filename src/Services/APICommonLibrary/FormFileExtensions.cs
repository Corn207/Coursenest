using Microsoft.AspNetCore.Http;

namespace APICommonLibrary;
public static class FormFileExtensions
{
    private static readonly MediaType[] _imageTypes = new[]
    {
        new MediaType("image/jpeg", new[] { ".jpg", ".jpeg", ".jfif", ".pjpeg", ".pjp" }),
        new MediaType("image/png", new[] { ".png" }),
        new MediaType("image/webp", new[] { ".webp" })
    };

    public static (string MIME, int? statusCode) GetImageMIME(this IFormFile file, long lengthLimit = 1024 * 1024 * 2)
    {
        if (file.Length > lengthLimit) return ("", 413);
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var type = _imageTypes.FirstOrDefault(t => t.Extensions.Contains(extension));
        if (type == null) return ("", 415);
        return (type.MIME, null);
    }
}

public record MediaType (string MIME, string[] Extensions);
