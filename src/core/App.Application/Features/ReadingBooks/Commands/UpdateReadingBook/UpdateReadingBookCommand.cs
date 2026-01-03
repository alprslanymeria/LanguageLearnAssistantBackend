using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Files;
using App.Application.Features.ReadingBooks.Dtos;

namespace App.Application.Features.ReadingBooks.Commands.UpdateReadingBook;

/// <summary>
/// COMMAND FOR UPDATING AN EXISTING READING BOOK.
/// </summary>
public record UpdateReadingBookCommand(
    int Id,
    int ReadingId,
    string Name,
    IFileUpload? ImageFile,
    IFileUpload? SourceFile,
    string UserId,
    int LanguageId
    ) : ICommand<ServiceResult<ReadingBookDto>>;
