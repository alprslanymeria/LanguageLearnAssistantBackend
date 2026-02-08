namespace App.Application.Features.ReadingOldSessions.Dtos;

/// <summary>
/// REQUEST DTO FOR SAVING A READING OLD SESSION.
/// </summary>
public record SaveReadingOldSessionRequest
(
    string Id,
    int ReadingId,
    int ReadingBookId,
    decimal Rate
);
