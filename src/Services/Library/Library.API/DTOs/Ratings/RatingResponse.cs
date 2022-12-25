namespace Library.API.DTOs.Ratings;

public record RatingResponse(
    int UserId,
    int Stars,
    DateTime Created,
    string Content
    );
