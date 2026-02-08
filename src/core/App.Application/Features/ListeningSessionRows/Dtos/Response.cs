using App.Application.Features.ListeningCategories.Dtos;

namespace App.Application.Features.ListeningSessionRows.Dtos;

/// <summary>
/// RESPONSE DTO FOR LISTENING SESSION ROW ENTITY.
/// </summary>
public record ListeningSessionRowDto
(
    int Id,
    string ListeningOldSessionId,
    string ListenedSentence,
    string Answer,
    decimal Similarity
);

/// <summary>
/// RESPONSE DTO FOR LISTENING SESSION ROWS.
/// </summary>
public record ListeningRowsResponse
(
    ListeningCategoryWithDeckVideos Item,
    List<ListeningSessionRowDto> Contents,
    int Total
);
