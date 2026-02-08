using App.Application.Features.ReadingBooks.Dtos;

namespace App.Application.Features.ReadingSessionRows.Dtos;

/// <summary>
/// RESPONSE DTO FOR READING SESSION ROW ENTITY.
/// </summary>
public record ReadingSessionRowDto
(
    int Id,
    string ReadingOldSessionId,
    string SelectedSentence,
    string Answer,
    string AnswerTranslate,
    decimal Similarity
);

/// <summary>
/// RESPONSE DTO FOR READING SESSION ROWS.
/// </summary>
public record ReadingRowsResponse
(
    ReadingBookDto Item,
    List<ReadingSessionRowDto> Contents,
    int Total
);
