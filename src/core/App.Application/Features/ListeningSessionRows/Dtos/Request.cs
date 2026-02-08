namespace App.Application.Features.ListeningSessionRows.Dtos;

/// <summary>
/// SINGLE ROW ITEM FOR BATCH SAVE.
/// </summary>
public record ListeningRowItemRequest
(
    string ListenedSentence,
    string Answer,
    decimal Similarity
);

/// <summary>
/// REQUEST DTO FOR SAVING LISTENING SESSION ROWS.
/// </summary>
public record SaveListeningRowsRequest
(
    string ListeningOldSessionId,
    List<ListeningRowItemRequest> Rows
);
