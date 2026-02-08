namespace App.Application.Features.FlashcardOldSessions.Dtos;

/// <summary>
/// RESPONSE DTO FOR FLASHCARD OLD SESSION ENTITY.
/// </summary>
public record FlashcardOldSessionDto
(
    string OldSessionId,
    int FlashcardId,
    int FlashcardCategoryId,
    decimal Rate,
    DateTime CreatedAt
);

/// <summary>
/// RESPONSE DTO FOR FLASHCARD OLD SESSION ENTITY WITH PAGING.
/// </summary>
public record FlashcardOldSessionWithTotalCount
(
    List<FlashcardOldSessionDto> FlashcardOldSessionDtos,
    int TotalCount
);
