namespace Library.API.DTOs.Ratings;

public record RatingPostRequest(
    int Stars,
    string Content
    );
