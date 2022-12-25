namespace Library.API.DTOs;

public record SubcategoryResponse(
    int SubcategoryId,
    string Content,
    List<TopicResponse> Topics
    );
