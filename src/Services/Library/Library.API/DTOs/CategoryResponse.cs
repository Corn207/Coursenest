namespace Library.API.DTOs;

public record CategoryResponse(
    int CategoryId,
    string Content,
    List<SubcategoryResponse> Subcategories
    );
