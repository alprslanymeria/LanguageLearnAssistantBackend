namespace App.Application.Features.ReadingSessionRows.Dtos;

/// <summary>
/// SINGLE ROW ITEM FOR BATCH SAVE.
/// </summary>
public record ReadingRowItemRequest
(
    string SelectedSentence,
    string Answer,
    string AnswerTranslate,
    decimal Similarity
);


/// <summary>
/// REQUEST DTO FOR SAVING READING SESSION ROWS.
/// </summary>
public record SaveReadingRowsRequest
(
    string ReadingOldSessionId,
    List<ReadingRowItemRequest> Rows
);
