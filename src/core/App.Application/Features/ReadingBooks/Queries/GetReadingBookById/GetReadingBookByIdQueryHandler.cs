using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.ReadingBooks.CacheKeys;
using App.Application.Features.ReadingBooks.Dtos;
using App.Domain.Entities.ReadingEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Features.ReadingBooks.Queries.GetReadingBookById;

/// <summary>
/// HANDLER FOR GET READING BOOK BY ID QUERY.
/// </summary>
public class GetReadingBookByIdQueryHandler(
    IReadingBookRepository readingBookRepository,
    IStaticCacheManager cacheManager,
    ICacheKeyFactory cacheKeyFactory,
    IMapper mapper,
    ILogger<GetReadingBookByIdQueryHandler> logger
    ) : IQueryHandler<GetReadingBookByIdQuery, ServiceResult<ReadingBookWithLanguageId>>
{
    public async Task<ServiceResult<ReadingBookWithLanguageId>> Handle(
        GetReadingBookByIdQuery request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("GetReadingBookByIdQueryHandler -> FETCHING READING BOOK WITH ID: {Id}", request.Id);

        var cacheKey = ReadingBookCacheKeys.ById(cacheKeyFactory, request.Id);

        var cachedResult = await cacheManager.GetAsync(cacheKey, async () =>
        {
            var readingBook = await readingBookRepository.GetReadingBookItemByIdAsync(request.Id);

            if (readingBook is null)
            {
                return null;
            }

            logger.LogInformation("GetReadingBookByIdQueryHandler -> SUCCESSFULLY FETCHED READING BOOK: {BookName}", readingBook.Name);

            return mapper.Map<ReadingBook, ReadingBookWithLanguageId>(readingBook);
        });

        if (cachedResult is null)
        {
            logger.LogWarning("GetReadingBookByIdQueryHandler -> READING BOOK NOT FOUND WITH ID: {Id}", request.Id);
            return ServiceResult<ReadingBookWithLanguageId>.Fail("READING BOOK NOT FOUND", HttpStatusCode.NotFound);
        }

        return ServiceResult<ReadingBookWithLanguageId>.Success(cachedResult);
    }
}
