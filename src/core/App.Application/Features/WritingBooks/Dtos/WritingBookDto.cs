using App.Application.Contracts.Infrastructure.Files;

namespace App.Application.Features.WritingBooks.Dtos;

/// <summary>
/// RESPONSE DTO FOR WRITING BOOK ENTITY.
/// </summary>
public record WritingBookDto
{
    public int WritingId { get; init; }
    public string Name { get; init; } = null!;
    public string ImageUrl { get; init; } = null!;
    public string LeftColor { get; init; } = null!;
    public string SourceUrl { get; init; } = null!;
}

/// <summary>
/// RESPONSE DTO FOR WRITING BOOK WITH WRITING INFORMATION.
/// </summary>
public record WritingBookWithLanguageId
{
    public int WritingId { get; init; }
    public string Name { get; init; } = null!;
    public string ImageUrl { get; init; } = null!;
    public string LeftColor { get; init; } = null!;
    public string SourceUrl { get; init; } = null!;
    public string LanguageName { get; init; } = null!;
}

/// <summary>
/// RESPONSE DTO FOR WRITING BOOK ENTITY WITH PAGING.
/// </summary>
public record WritingBookWithTotalCount
{
    public List<WritingBookDto> WritingBookDtos { get; set; } = [];
    public int TotalCount { get; init; }
}

/// <summary>
/// REQUEST DTO FOR CREATING A WRITING BOOK.
/// </summary>
public record CreateWritingBookRequest
{
    public int WritingId { get; init; }
    public string Name { get; init; } = null!;
    public IFileUpload ImageFile { get; init; } = null!;
    public IFileUpload SourceFile { get; init; } = null!;
    public string UserId { get; init; } = null!;
    public int LanguageId { get; init; }
}

/// <summary>
/// REQUEST DTO FOR UPDATING A WRITING BOOK.
/// </summary>
public record UpdateWritingBookRequest
{
    public int Id { get; init; }
    public int WritingId { get; init; }
    public string Name { get; init; } = null!;
    public IFileUpload? ImageFile { get; init; }
    public IFileUpload? SourceFile { get; init; }
    public string UserId { get; init; } = null!;
    public int LanguageId { get; init; }
}
