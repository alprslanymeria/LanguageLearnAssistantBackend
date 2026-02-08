namespace App.Application.Features.ReadingBooks.Dtos;

/// <summary>
/// RESPONSE DTO FOR READING BOOK ENTITY.
/// </summary>
public record ReadingBookDto
(
    int Id,
    int ReadingId,
    string Name,
    string ImageUrl,
    string LeftColor,
    string SourceUrl
);

/// <summary>
/// RESPONSE DTO FOR READING BOOK WITH READING INFORMATION.
/// </summary>
public record ReadingBookWithLanguageId
(
    int Id,
    int ReadingId,
    string Name,
    string ImageUrl,
    string LeftColor,
    string SourceUrl,
    int LanguageId
);

/// <summary>
/// RESPONSE DTO FOR READING BOOK ENTITY WITH PAGING.
/// </summary>
public record ReadingBookWithTotalCount
(
    List<ReadingBookDto> ReadingBookDtos,
    int TotalCount
);
