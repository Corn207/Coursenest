namespace Identity.API.Models;

public class Avatar
{
    public Avatar(string mediaType, byte[] data)
    {
        MediaType = mediaType;
        Data = data;
    }

    public int AvatarId { get; set; }
    public string MediaType { get; set; }
    public byte[] Data { get; set; }

    // Relationship
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
