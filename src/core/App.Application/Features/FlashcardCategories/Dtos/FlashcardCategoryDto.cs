using App.Application.Features.ReadingBooks.Dtos;

namespace App.Application.Features.FlashcardCategories.Dtos;

/// <summary>
/// RESPONSE DTO FOR FLASHCARD CATEGORY ENTITY.
/// </summary>
public record FlashcardCategoryDto
{
    public int FlashcardId { get; init; }
    public string Name { get; init; } = null!;
}

/// <summary>
/// RESPONSE DTO FOR FLASHCARD CATEGORY WITH DETAILS.
/// </summary>
public record FlashcardCategoryWithLanguageId
{
    public int FlashcardId { get; init; }
    public string Name { get; init; } = null!;
    public int LanguageId { get; init; }
}


/// <summary>
/// RESPONSE DTO FOR READING BOOK ENTITY WITH PAGING.
/// </summary>
public record FlashcardCategoryWithTotalCount
{
    public List<FlashcardCategoryDto> FlashcardCategoryDtos { get; set; } = [];
    public int TotalCount { get; init; }
}


/// <summary>
/// REQUEST DTO FOR CREATING A FLASHCARD CATEGORY.
/// </summary>
public record CreateFlashcardCategoryRequest
{
    public int FlashcardId { get; init; }
    public string Name { get; init; } = null!;
    public string UserId { get; init; } = null!;
    public int LanguageId { get; init; }
}

/// <summary>
/// REQUEST DTO FOR UPDATING A FLASHCARD CATEGORY.
/// </summary>
public record UpdateFlashcardCategoryRequest
{
    public int Id { get; init; }
    public int FlashcardId { get; init; }
    public string Name { get; init; } = null!;
    public string UserId { get; init; } = null!;
    public int LanguageId { get; init; }
}