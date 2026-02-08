using App.Application.Features.DeckVideo.Dtos;

namespace App.Application.Features.ListeningCategories.Dtos;

/// <summary>
/// RESPONSE DTO FOR LISTENING CATEGORY ENTITY.
/// </summary>
public record ListeningCategoryDto
(
    int Id,
    int ListeningId,
    string Name
);

/// <summary>
/// RESPONSE DTO FOR LISTENING CATEGORY ENTITY WITH DECK VIDEOS.
/// </summary>
public record ListeningCategoryWithDeckVideos
(
    int Id,
    string Name,
    int ListeningId,
    List<DeckVideoDto> DeckVideos
);
