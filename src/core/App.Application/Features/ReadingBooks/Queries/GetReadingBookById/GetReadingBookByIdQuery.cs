using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.ReadingBooks.Dtos;

namespace App.Application.Features.ReadingBooks.Queries.GetReadingBookById;

/// <summary>
/// QUERY FOR RETRIEVING A READING BOOK BY ID.
/// </summary>
public record GetReadingBookByIdQuery(int Id) : IQuery<ServiceResult<ReadingBookWithLanguageId>>;
