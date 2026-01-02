using App.Application.Features.ReadingOldSessions.Dtos;

namespace App.Application.Features.WritingOldSessions.Dtos;

/// <summary>
/// RESPONSE DTO FOR WRITING OLD SESSION ENTITY.
/// </summary>
public record WritingOldSessionDto
{
    public int WritingId { get; init; }
    public int WritingBookId { get; init; }
    public decimal Rate { get; init; }
    public DateTime CreatedAt { get; init; }
}

/// <summary>
/// RESPONSE DTO FOR WRITING OLD SESSION ENTITY WITH PAGING.
/// </summary>
public record WritingOldSessionWithTotalCount
{
    public List<WritingOldSessionDto> WritingOldSessionDtos { get; set; } = [];
    public int TotalCount { get; init; }
}

/// <summary>
/// REQUEST DTO FOR SAVING A WRITING OLD SESSION.
/// </summary>
public record SaveWritingOldSessionRequest
{
    public string Id { get; init; } = null!;
    public int WritingId { get; init; }
    public int WritingBookId { get; init; }
    public decimal Rate { get; init; }
}
