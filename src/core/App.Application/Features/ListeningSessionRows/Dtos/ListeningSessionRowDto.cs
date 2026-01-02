using App.Domain.Entities.ListeningEntities;

namespace App.Application.Features.ListeningSessionRows.Dtos;

/// <summary>
/// RESPONSE DTO FOR LISTENING SESSION ROW ENTITY.
/// </summary>
public record ListeningSessionRowDto
{
    public string ListeningOldSessionId { get; init; } = null!;
    public string ListenedSentence { get; init; } = null!;
    public string Answer { get; init; } = null!;
    public decimal Similarity { get; init; }
}

/// <summary>
/// RESPONSE DTO FOR LISTENING SESSION ROWS.
/// </summary>
public record ListeningRowsResponse
{
    public ListeningCategory Item { get; set; } = null!;
    public ICollection<ListeningSessionRowDto> Contents { get; set; } = null!;
    public int Total { get; set; }
}

/// <summary>
/// SINGLE ROW ITEM FOR BATCH SAVE.
/// </summary>
public record ListeningRowItemRequest
{
    public string ListenedSentence { get; init; } = null!;
    public string Answer { get; init; } = null!;
    public decimal Similarity { get; init; }
}

/// <summary>
/// REQUEST DTO FOR SAVING LISTENING SESSION ROWS.
/// </summary>
public record SaveListeningRowsRequest
{
    public string ListeningOldSessionId { get; init; } = null!;
    public List<ListeningRowItemRequest> Rows { get; init; } = [];
}