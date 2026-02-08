namespace App.Application.Features.ReadingOldSessions.Dtos;

/// <summary>
/// RESPONSE DTO FOR READING OLD SESSION ENTITY.
/// </summary>
public record ReadingOldSessionDto
(
    string OldSessionId,
    int ReadingId,
    int ReaidingBookId,
    decimal Rate,
    DateTime CreatedAt
);

/// <summary>
/// RESPONSE DTO FOR READING OLD SESSION ENTITY WITH PAGING.
/// </summary>
public record ReadingOldSessionWithTotalCount
(
    List<ReadingOldSessionDto> ReadingOldSessionDtos,
    int TotalCount
);
