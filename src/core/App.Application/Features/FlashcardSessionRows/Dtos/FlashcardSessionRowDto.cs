using App.Domain.Entities.FlashcardEntities;

namespace App.Application.Features.FlashcardSessionRows.Dtos;

/// <summary>
/// RESPONSE DTO FOR FLASHCARD SESSION ROW ENTITY.
/// </summary>
public record FlashcardSessionRowDto
{
    public string FlashcardOldSessionId { get; init; } = null!;
    public string Question { get; init; } = null!;
    public string Answer { get; init; } = null!;
    public bool Status { get; init; }
}

/// <summary>
/// RESPONSE DTO FOR FLASHCARD SESSION ROWS.
/// </summary>
public record FlashcardRowsResponse
{
    public FlashcardCategory Item { get; set; } = null!;
    public ICollection<FlashcardSessionRowDto> Contents { get; set; } = null!;
    public int Total { get; set; }
}

/// <summary>
/// SINGLE ROW ITEM FOR BATCH SAVE.
/// </summary>
public record FlashcardRowItemRequest
{
    public string Question { get; init; } = null!;
    public string Answer { get; init; } = null!;
    public bool Status { get; init; }
}


/// <summary>
/// REQUEST DTO FOR SAVING FLASHCARD SESSION ROWS.
/// </summary>
public record SaveFlashcardRowsRequest
{
    public string FlashcardOldSessionId { get; init; } = null!;
    public List<FlashcardRowItemRequest> Rows { get; init; } = [];
}