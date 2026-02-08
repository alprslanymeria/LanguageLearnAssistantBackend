using App.Application.Features.DeckWords.Dtos;

namespace App.Application.Features.FlashcardCategories.Dtos;

/// <summary>
/// RESPONSE DTO FOR FLASHCARD CATEGORY ENTITY.
/// </summary>
public record FlashcardCategoryDto
(
    int Id,
    int FlashcardId,
    string Name
);

/// <summary>
/// RESPONSE DTO FOR FLASHCARD CATEGORY WITH DECK WORDS.
/// </summary>
public record FlashcardCategoryWithDeckWords
(
    int Id,
    int FlashcardId,
    string Name,
    List<DeckWordDto> DeckWords
);

/// <summary>
/// RESPONSE DTO FOR FLASHCARD CATEGORY WITH DETAILS.
/// </summary>
public record FlashcardCategoryWithLanguageId
(
    int Id,
    int FlashcardId,
    string Name,
    int LanguageId
);

/// <summary>
/// RESPONSE DTO FOR READING BOOK ENTITY WITH PAGING.
/// </summary>
public record FlashcardCategoryWithTotalCount
(
    List<FlashcardCategoryDto> FlashcardCategoryDtos,
    int TotalCount
);

/// <summary>
/// RESPONSE DTO FOR READING BOOK ENTITY WITH LANGUAGE Ids.
/// </summary>
public record FlashcardCategoryWithLanguageIds
(
    List<FlashcardCategoryWithLanguageId> FlashcardCategoryDtos,
    int TotalCount
);
