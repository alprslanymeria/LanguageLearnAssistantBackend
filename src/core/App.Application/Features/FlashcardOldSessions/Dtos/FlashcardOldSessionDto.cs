using App.Application.Features.ReadingOldSessions.Dtos;

namespace App.Application.Features.FlashcardOldSessions.Dtos;

/// <summary>
/// RESPONSE DTO FOR FLASHCARD OLD SESSION ENTITY.
/// </summary>
public record FlashcardOldSessionDto
{
    public int FlashcardId { get; init; }
    public int FlashcardCategoryId { get; init; }
    public decimal Rate { get; init; }
    public DateTime CreatedAt { get; init; }
}

/// <summary>
/// RESPONSE DTO FOR FLASHCARD OLD SESSION ENTITY WITH PAGING.
/// </summary>
public record FlashcardOldSessionWithTotalCount
{
    public List<FlashcardOldSessionDto> FlashcardOldSessionDtos { get; set; } = [];
    public int TotalCount { get; init; }
}

/// <summary>
/// REQUEST DTO FOR SAVING A FLASHCARD OLD SESSION.
/// </summary>
public record SaveFlashcardOldSessionRequest
{
    public string Id { get; init; } = null!;
    public int FlashcardId { get; init; }
    public int FlashcardCategoryId { get; init; }
    public decimal Rate { get; init; }
}
