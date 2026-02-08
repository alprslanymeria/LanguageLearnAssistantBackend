namespace App.Application.Features.DeckWords.Dtos;

/// <summary>
/// RESPONSE DTO FOR DECK WORD ENTITY.
/// </summary>
public record DeckWordDto
(
    int Id,
    int CategoryId,
    string Question,
    string Answer
);

/// <summary>
/// RESPONSE DTO FOR DECK WORD WITH DETAILS.
/// </summary>
public record DeckWordWithLanguageId
(
    int Id,
    int CategoryId,
    string Question,
    string Answer,
    int LanguageId
);

/// <summary>
/// RESPONSE DTO FOR DECK WORD ENTITY WITH PAGING.
/// </summary>
public record DeckWordWithTotalCount
(
    List<DeckWordDto> DeckWordDtos,
    int TotalCount
);
