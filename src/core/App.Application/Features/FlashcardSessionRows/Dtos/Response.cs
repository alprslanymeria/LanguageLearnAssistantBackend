using App.Application.Features.FlashcardCategories.Dtos;

namespace App.Application.Features.FlashcardSessionRows.Dtos;

/// <summary>
/// RESPONSE DTO FOR FLASHCARD SESSION ROW ENTITY.
/// </summary>
public record FlashcardSessionRowDto
(
    int Id,
    string FlashcardOldSessionId,
    string Question,
    string Answer,
    bool Status
);

/// <summary>
/// RESPONSE DTO FOR FLASHCARD SESSION ROWS.
/// </summary>
public record FlashcardRowsResponse
(
    FlashcardCategoryWithDeckWords Item,
    List<FlashcardSessionRowDto> Contents,
    int Total
);
