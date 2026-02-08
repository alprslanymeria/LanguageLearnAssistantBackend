using App.Application.Features.WritingBooks.Dtos;

namespace App.Application.Features.WritingSessionRows.Dtos;

/// <summary>
/// RESPONSE DTO FOR WRITING SESSION ROW ENTITY.
/// </summary>
public record WritingSessionRowDto
(
    int Id,
    string WritingOldSessionId,
    string SelectedSentence,
    string Answer,
    string AnswerTranslate,
    decimal Similarity
);

/// <summary>
/// RESPONSE DTO FOR WRITING SESSION ROWS.
/// </summary>
public record WritingRowsResponse
(
    WritingBookDto Item,
    List<WritingSessionRowDto> Contents,
    int Total
);
