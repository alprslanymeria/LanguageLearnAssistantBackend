namespace App.Application.Features.ListeningOldSessions.Dtos;

/// <summary>
/// REQUEST DTO FOR SAVING A LISTENING OLD SESSION.
/// </summary>
public record SaveListeningOldSessionRequest
(
    string Id,
    int ListeningId,
    int ListeningCategoryId,
    decimal Rate
);
