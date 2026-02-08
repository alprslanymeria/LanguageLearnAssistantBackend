using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.WritingBooks.Dtos;

namespace App.Application.Features.WritingBooks.Commands.UpdateWritingBook;

/// <summary>
/// COMMAND FOR UPDATING AN EXISTING WRITING BOOK.
/// </summary>
public record UpdateWritingBookCommand(UpdateWritingBookRequest Request) : ICommand<ServiceResult>;
