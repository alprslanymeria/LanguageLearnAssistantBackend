using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Files;
using App.Application.Features.WritingBooks.Dtos;

namespace App.Application.Features.WritingBooks.Commands.UpdateWritingBook;

/// <summary>
/// COMMAND FOR UPDATING AN EXISTING WRITING BOOK.
/// </summary>
public record UpdateWritingBookCommand(
    int Id,
    int WritingId,
    string Name,
    IFileUpload? ImageFile,
    IFileUpload? SourceFile,
    string UserId,
    int LanguageId
    ) : ICommand<ServiceResult<WritingBookDto>>;
