using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.ReadingBooks.Dtos;
using App.Domain.Entities.ReadingEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.ReadingBooks.Queries.GetReadingBookById;

/// <summary>
/// HANDLER FOR GET READING BOOK BY ID QUERY.
/// </summary>
public class GetReadingBookByIdQueryHandler(

    IReadingBookRepository readingBookRepository,
    IMapper mapper,
    ILogger<GetReadingBookByIdQueryHandler> logger

    ) : IQueryHandler<GetReadingBookByIdQuery, ServiceResult<ReadingBookWithLanguageId>>
{
    public async Task<ServiceResult<ReadingBookWithLanguageId>> Handle(

        GetReadingBookByIdQuery request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("GetReadingBookByIdQueryHandler --> FETCHING READING BOOK WITH ID: {Id}", request.Id);

        var readingBook = await readingBookRepository.GetReadingBookItemByIdAsync(request.Id);

        if (readingBook is null)
        {
            logger.LogWarning("GetReadingBookByIdQueryHandler --> READING BOOK NOT FOUND WITH ID: {Id}", request.Id);
            return ServiceResult<ReadingBookWithLanguageId>.Fail("READING BOOK NOT FOUND", HttpStatusCode.NotFound);
        }

        logger.LogInformation("GetReadingBookByIdQueryHandler --> SUCCESSFULLY FETCHED READING BOOK: {BookName}", readingBook.Name);

        var result = mapper.Map<ReadingBook, ReadingBookWithLanguageId>(readingBook);
        return ServiceResult<ReadingBookWithLanguageId>.Success(result);
    }
}
