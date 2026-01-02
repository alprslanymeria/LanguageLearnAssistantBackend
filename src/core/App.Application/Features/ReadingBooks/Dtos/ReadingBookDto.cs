using App.Application.Contracts.Infrastructure.Files;

namespace App.Application.Features.ReadingBooks.Dtos;

/// <summary>
/// RESPONSE DTO FOR READING BOOK ENTITY.
/// </summary>
public record ReadingBookDto
{
    public int ReadingId { get; init; }
    public string Name { get; init; } = null!;
    public string ImageUrl { get; init; } = null!;
    public string LeftColor { get; init; } = null!;
    public string SourceUrl { get; init; } = null!;
}

/// <summary>
/// RESPONSE DTO FOR READING BOOK WITH READING INFORMATION.
/// </summary>
public record ReadingBookWithLanguageId
{
    public int ReadingId { get; init; }
    public string Name { get; init; } = null!;
    public string ImageUrl { get; init; } = null!;
    public string LeftColor { get; init; } = null!;
    public string SourceUrl { get; init; } = null!;
    public int LanguageId { get; init; }
}

/// <summary>
/// RESPONSE DTO FOR READING BOOK ENTITY WITH PAGING.
/// </summary>
public record ReadingBookWithTotalCount
{
    public List<ReadingBookDto> ReadingBookDtos { get; set; } = [];
    public int TotalCount { get; init; }
}

/// <summary>
/// REQUEST DTO FOR CREATING READING BOOK.
/// </summary>
public record CreateReadingBookRequest
{
    public int ReadingId { get; init; }
    public string Name { get; init; } = null!;
    public IFileUpload ImageFile { get; init; } = null!;
    public IFileUpload SourceFile { get; init; } = null!;
    public string UserId { get; init; } = null!;
    public int LanguageId { get; init; }
}

/// <summary>
/// REQUEST DTO FOR UPDATING READING BOOK.
/// </summary>
public record UpdateReadingBookRequest
{
    public int Id { get; init; }
    public int ReadingId { get; init; }
    public string Name { get; init; } = null!;
    public IFileUpload? ImageFile { get; init; }
    public IFileUpload? SourceFile { get; init; }
    public string UserId { get; init; } = null!;
    public int LanguageId { get; init; }
}
