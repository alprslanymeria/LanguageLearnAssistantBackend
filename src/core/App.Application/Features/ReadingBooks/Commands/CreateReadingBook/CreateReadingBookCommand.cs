using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.ReadingBooks.Dtos;

namespace App.Application.Features.ReadingBooks.Commands.CreateReadingBook;

/// <summary>
/// COMMAND FOR CREATING A NEW READING BOOK.
/// </summary>
public record CreateReadingBookCommand(CreateReadingBookRequest Request) : ICommand<ServiceResult>;
