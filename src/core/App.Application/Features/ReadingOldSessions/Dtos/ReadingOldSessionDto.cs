namespace App.Application.Features.ReadingOldSessions.Dtos;

/// <summary>
/// RESPONSE DTO FOR READING OLD SESSION ENTITY.
/// </summary>
public record ReadingOldSessionDto
{
    public int ReadingId { get; init; }
    public int ReadingBookId { get; init; }
    public decimal Rate { get; init; }
    public DateTime CreatedAt { get; init; }
}

/// <summary>
/// RESPONSE DTO FOR READING OLD SESSION ENTITY WITH PAGING.
/// </summary>
public record ReadingOldSessionWithTotalCount
{
    public List<ReadingOldSessionDto> ReadingOldSessionDtos { get; set; } = [];
    public int TotalCount { get; init; }
}


/// <summary>
/// REQUEST DTO FOR SAVING A READING OLD SESSION.
/// </summary>
public record SaveReadingOldSessionRequest
{
    public string Id { get; init; } = null!;
    public int ReadingId { get; init; }
    public int ReadingBookId { get; init; }
    public decimal Rate { get; init; }
}
