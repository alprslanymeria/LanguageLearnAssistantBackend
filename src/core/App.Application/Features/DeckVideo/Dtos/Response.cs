namespace App.Application.Features.DeckVideo.Dtos;

/// <summary>
/// RESPONSE DTO FOR DECK VIDEO ENTITY.
/// </summary>
public record DeckVideoDto
(
    int Id,
    int CategoryId,
    string Correct,
    string SourceUrl
);
