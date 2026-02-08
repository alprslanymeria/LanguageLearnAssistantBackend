namespace App.Application.Features.ListeningOldSessions.Dtos;

/// <summary>
/// RESPONSE DTO FOR LISTENING OLD SESSION ENTITY.
/// </summary>
public record ListeningOldSessionDto
(
    string OldSessionId,
    int ListeningId,
    int ListeningCategoryId,
    decimal Rate,
    DateTime CreatedAt
);

/// <summary>
/// RESPONSE DTO FOR LISTENING OLD SESSION ENTITY WITH PAGING.
/// </summary>
public record ListeningOldSessionWithTotalCount
(
    List<ListeningOldSessionDto> ListeningOldSessionDtos,
    int TotalCount
);
