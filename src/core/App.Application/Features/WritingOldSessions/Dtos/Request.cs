namespace App.Application.Features.WritingOldSessions.Dtos;

/// <summary>
/// REQUEST DTO FOR SAVING A WRITING OLD SESSION.
/// </summary>
public record SaveWritingOldSessionRequest
(
    string Id,
    int WritingId,
    int WritingBookId,
    decimal Rate
);
