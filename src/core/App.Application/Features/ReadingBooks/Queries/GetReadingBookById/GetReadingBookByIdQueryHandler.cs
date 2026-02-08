using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.ReadingBooks.Dtos;
using App.Domain.Exceptions;

namespace App.Application.Features.ReadingBooks.Queries.GetReadingBookById;

/// <summary>
/// HANDLER FOR GET READING BOOK BY ID QUERY.
/// </summary>
public class GetReadingBookByIdQueryHandler(

    IReadingBookRepository readingBookRepository

    ) : IQueryHandler<GetReadingBookByIdQuery, ServiceResult<ReadingBookWithLanguageId>>
{
    public async Task<ServiceResult<ReadingBookWithLanguageId>> Handle(

        GetReadingBookByIdQuery request,
        CancellationToken cancellationToken)
    {

        // GET READING BOOK
        var readingBook = await readingBookRepository.GetReadingBookItemByIdAsync(request.Id)
            ?? throw new NotFoundException("READING BOOK NOT FOUND");

        return ServiceResult<ReadingBookWithLanguageId>.Success(readingBook);
    }
}
