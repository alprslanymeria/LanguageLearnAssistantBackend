using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.WritingBooks.Dtos;

namespace App.Application.Features.WritingBooks.Queries.GetWritingBookById;

/// <summary>
/// QUERY FOR RETRIEVING A WRITING BOOK BY ID.
/// </summary>
public record GetWritingBookByIdQuery(int Id) : IQuery<ServiceResult<WritingBookWithLanguageId>>;
