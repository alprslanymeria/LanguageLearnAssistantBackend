using App.Domain.Entities.ReadingEntities;

namespace App.Application.Features.ReadingSessionRows.Dtos;

/// <summary>
/// RESPONSE DTO FOR READING SESSION ROW ENTITY.
/// </summary>
public record ReadingSessionRowDto
{
    public string ReadingOldSessionId { get; init; } = null!;
    public string SelectedSentence { get; init; } = null!;
    public string Answer { get; init; } = null!;
    public string AnswerTranslate { get; init; } = null!;
    public decimal Similarity { get; init; }
}

/// <summary>
/// RESPONSE DTO FOR READING SESSION ROWS.
/// </summary>
public record ReadingRowsResponse
{
    public ReadingBook Item { get; set; } = null!;
    public ICollection<ReadingSessionRowDto> Contents { get; set; } = null!;
    public int Total { get; set; }
}


/// <summary>
/// SINGLE ROW ITEM FOR BATCH SAVE.
/// </summary>
public record ReadingRowItemRequest
{
    public string SelectedSentence { get; init; } = null!;
    public string Answer { get; init; } = null!;
    public string AnswerTranslate { get; init; } = null!;
    public decimal Similarity { get; init; }
}


/// <summary>
/// REQUEST DTO FOR SAVING READING SESSION ROWS.
/// </summary>
public record SaveReadingRowsRequest
{
    public string ReadingOldSessionId { get; init; } = null!;
    public List<ReadingRowItemRequest> Rows { get; init; } = [];
}