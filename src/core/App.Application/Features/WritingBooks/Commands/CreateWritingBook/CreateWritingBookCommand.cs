using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.WritingBooks.Dtos;

namespace App.Application.Features.WritingBooks.Commands.CreateWritingBook;

/// <summary>
/// COMMAND FOR CREATING A NEW WRITING BOOK.
/// </summary>
public record CreateWritingBookCommand(CreateWritingBookRequest Request) : ICommand<ServiceResult<int>>;
