using App.Application.Features.ReadingBooks.Dtos;

namespace App.Application.Features.DeckWords.Dtos;

/// <summary>
/// RESPONSE DTO FOR DECK WORD ENTITY.
/// </summary>
public record DeckWordDto
{
    public int FlashcardCategoryId { get; init; }
    public string Question { get; init; } = null!;
    public string Answer { get; init; } = null!;
}

/// <summary>
/// RESPONSE DTO FOR DECK WORD WITH DETAILS.
/// </summary>
public record DeckWordWithLanguageId
{
    public int FlashcardCategoryId { get; init; }
    public string Question { get; init; } = null!;
    public string Answer { get; init; } = null!;
    public int LanguageId { get; init; }
}

/// <summary>
/// RESPONSE DTO FOR DECK WORD ENTITY WITH PAGING.
/// </summary>
public record DeckWordWithTotalCount
{
    public List<DeckWordDto> DeckWordDtos { get; set; } = [];
    public int TotalCount { get; init; }
}


/// <summary>
/// REQUEST DTO FOR CREATING A DECK WORD.
/// </summary>
public record CreateDeckWordRequest
{
    public int FlashcardCategoryId { get; init; }
    public string Question { get; init; } = null!;
    public string Answer { get; init; } = null!;
}

/// <summary>
/// REQUEST DTO FOR UPDATING A DECK WORD.
/// </summary>
public record UpdateDeckWordRequest
{
    public int Id { get; init; }
    public int FlashcardCategoryId { get; init; }
    public string Question { get; init; } = null!;
    public string Answer { get; init; } = null!;
}
