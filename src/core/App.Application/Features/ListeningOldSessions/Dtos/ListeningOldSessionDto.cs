namespace App.Application.Features.ListeningOldSessions.Dtos;

/// <summary>
/// RESPONSE DTO FOR LISTENING OLD SESSION ENTITY.
/// </summary>
public record ListeningOldSessionDto
{
    public int ListeningId { get; init; }
    public int ListeningCategoryId { get; init; }
    public decimal Rate { get; init; }
    public DateTime CreatedAt { get; init; }
}

/// <summary>
/// RESPONSE DTO FOR LISTENING OLD SESSION ENTITY WITH PAGING.
/// </summary>
public record ListeningOldSessionWithTotalCount
{
    public List<ListeningOldSessionDto> ListeningOldSessionDtos { get; set; } = [];
    public int TotalCount { get; init; }
}

/// <summary>
/// REQUEST DTO FOR SAVING A LISTENING OLD SESSION.
/// </summary>
public record SaveListeningOldSessionRequest
{
    public string Id { get; init; } = null!;
    public int ListeningId { get; init; }
    public int ListeningCategoryId { get; init; }
    public decimal Rate { get; init; }
}
