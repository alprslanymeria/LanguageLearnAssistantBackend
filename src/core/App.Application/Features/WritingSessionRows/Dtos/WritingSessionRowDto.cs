using App.Domain.Entities.WritingEntities;

namespace App.Application.Features.WritingSessionRows.Dtos;

/// <summary>
/// RESPONSE DTO FOR WRITING SESSION ROW ENTITY.
/// </summary>
public record WritingSessionRowDto
{
    public string WritingOldSessionId { get; init; } = null!;
    public string SelectedSentence { get; init; } = null!;
    public string Answer { get; init; } = null!;
    public string AnswerTranslate { get; init; } = null!;
    public decimal Similarity { get; init; }
}

/// <summary>
/// RESPONSE DTO FOR WRITING SESSION ROWS.
/// </summary>
public record WritingRowsResponse
{
    public WritingBook Item { get; set; } = null!;
    public ICollection<WritingSessionRowDto> Contents { get; set; } = null!;
    public int Total { get; set; }
}


/// <summary>
/// SINGLE ROW ITEM FOR BATCH SAVE.
/// </summary>
public record WritingRowItemRequest
{
    public string SelectedSentence { get; init; } = null!;
    public string Answer { get; init; } = null!;
    public string AnswerTranslate { get; init; } = null!;
    public decimal Similarity { get; init; }
}

/// <summary>
/// REQUEST DTO FOR SAVING WRITING SESSION ROWS.
/// </summary>
public record SaveWritingRowsRequest
{
    public string WritingOldSessionId { get; init; } = null!;
    public List<WritingRowItemRequest> Rows { get; init; } = [];
}