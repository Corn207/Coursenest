namespace Library.API.Models;

public class Image
{
    public Image(string mediaType, byte[] data)
    {
        MediaType = mediaType;
        Data = data;
    }

    public int Id { get; set; }
    public string MediaType { get; set; }
    public byte[] Data { get; set; }
}
