using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models;

public class Avatar
{
    public Avatar(string mediaType, byte[] data)
    {
        MediaType = mediaType;
        Data = data;
    }

    public string MediaType { get; set; }
    public byte[] Data { get; set; }

    // Relationship
    [Key]
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
