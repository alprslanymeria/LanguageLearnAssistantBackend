namespace App.Application.Features.WritingBooks.Dtos;

/// <summary>
/// RESPONSE DTO FOR WRITING BOOK ENTITY.
/// </summary>
public record WritingBookDto
(
    int Id,
    int WritingId,
    string Name,
    string ImageUrl,
    string LeftColor,
    string SourceUrl
);

/// <summary>
/// RESPONSE DTO FOR WRITING BOOK WITH WRITING INFORMATION.
/// </summary>
public record WritingBookWithLanguageId
(
    int Id,
    int WritingId,
    string Name,
    string ImageUrl,
    string LeftColor,
    string SourceUrl,
    int LanguageId
);

/// <summary>
/// RESPONSE DTO FOR WRITING BOOK ENTITY WITH PAGING.
/// </summary>
public record WritingBookWithTotalCount
(
    List<WritingBookDto> WritingBookDtos,
    int TotalCount
);
