using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.ReadingBooks.Dtos;

namespace App.Application.Features.ReadingBooks.Commands.UpdateReadingBook;

/// <summary>
/// COMMAND FOR UPDATING AN EXISTING READING BOOK.
/// </summary>
public record UpdateReadingBookCommand(UpdateReadingBookRequest Request) : ICommand<ServiceResult<int>>;
