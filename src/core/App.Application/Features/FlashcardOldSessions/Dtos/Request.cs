namespace App.Application.Features.FlashcardOldSessions.Dtos;

/// <summary>
/// REQUEST DTO FOR SAVING A FLASHCARD OLD SESSION.
/// </summary>
public record SaveFlashcardOldSessionRequest
(
    string Id,
    int FlashcardId,
    int FlashcardCategoryId,
    decimal Rate
);
