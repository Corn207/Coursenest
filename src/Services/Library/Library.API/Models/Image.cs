using System.ComponentModel.DataAnnotations;

namespace Library.API.Models;

public class Image
{
    public Image(string mediaType, byte[] data)
    {
        MediaType = mediaType;
        Data = data;
    }

    public string MediaType { get; set; }
    public byte[] Data { get; set; }

    // Relationship
    [Key]
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
}
