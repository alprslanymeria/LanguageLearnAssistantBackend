namespace App.Application.Features.WritingOldSessions.Dtos;

/// <summary>
/// RESPONSE DTO FOR WRITING OLD SESSION ENTITY.
/// </summary>
public record WritingOldSessionDto
(
    string OldSessionId,
    int WritingId,
    int WritingBookId,
    decimal Rate,
    DateTime CreatedAt
);


/// <summary>
/// RESPONSE DTO FOR WRITING OLD SESSION ENTITY WITH PAGING.
/// </summary>
public record WritingOldSessionWithTotalCount
(
    List<WritingOldSessionDto> WritingOldSessionDtos,
    int TotalCount
);
