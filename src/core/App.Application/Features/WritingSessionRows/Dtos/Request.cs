namespace App.Application.Features.WritingSessionRows.Dtos;

/// <summary>
/// SINGLE ROW ITEM FOR BATCH SAVE.
/// </summary>
public record WritingRowItemRequest
(
    string SelectedSentence,
    string Answer,
    string AnswerTranslate,
    decimal Similarity
);

/// <summary>
/// REQUEST DTO FOR SAVING WRITING SESSION ROWS.
/// </summary>
public record SaveWritingRowsRequest
(
    string WritingOldSessionId,
    List<WritingRowItemRequest> Rows
);
