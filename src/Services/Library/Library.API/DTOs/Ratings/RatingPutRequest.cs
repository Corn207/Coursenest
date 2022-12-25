namespace Library.API.DTOs.Ratings;

public record RatingPutRequest(
    int CourseId,
    int? Stars,
    string? Content
    );
