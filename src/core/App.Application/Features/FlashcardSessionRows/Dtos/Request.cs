namespace App.Application.Features.FlashcardSessionRows.Dtos;

/// <summary>
/// SINGLE ROW ITEM FOR BATCH SAVE.
/// </summary>
public record FlashcardRowItemRequest
(
    string Question,
    string Answer,
    bool Status
);


/// <summary>
/// REQUEST DTO FOR SAVING FLASHCARD SESSION ROWS.
/// </summary>
public record SaveFlashcardRowsRequest
(
    string FlashcardOldSessionId,
    List<FlashcardRowItemRequest> Rows
);
