using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.WritingBooks.CacheKeys;
using App.Application.Features.WritingBooks.Dtos;
using App.Domain.Entities.WritingEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Features.WritingBooks.Queries.GetWritingBookById;

/// <summary>
/// HANDLER FOR GET WRITING BOOK BY ID QUERY.
/// </summary>
public class GetWritingBookByIdQueryHandler(
    IWritingBookRepository writingBookRepository,
    IStaticCacheManager cacheManager,
    ICacheKeyFactory cacheKeyFactory,
    IMapper mapper,
    ILogger<GetWritingBookByIdQueryHandler> logger
    ) : IQueryHandler<GetWritingBookByIdQuery, ServiceResult<WritingBookWithLanguageId>>
{
    public async Task<ServiceResult<WritingBookWithLanguageId>> Handle(
        GetWritingBookByIdQuery request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("GetWritingBookByIdQueryHandler -> FETCHING WRITING BOOK WITH ID: {Id}", request.Id);

        var cacheKey = WritingBookCacheKeys.ById(cacheKeyFactory, request.Id);

        var cachedResult = await cacheManager.GetAsync(cacheKey, async () =>
        {
            var writingBook = await writingBookRepository.GetWritingBookItemByIdAsync(request.Id);

            if (writingBook is null)
            {
                return null;
            }

            logger.LogInformation("GetWritingBookByIdQueryHandler -> SUCCESSFULLY FETCHED WRITING BOOK: {BookName}", writingBook.Name);

            return mapper.Map<WritingBook, WritingBookWithLanguageId>(writingBook);
        });

        if (cachedResult is null)
        {
            logger.LogWarning("GetWritingBookByIdQueryHandler -> WRITING BOOK NOT FOUND WITH ID: {Id}", request.Id);
            return ServiceResult<WritingBookWithLanguageId>.Fail("WRITING BOOK NOT FOUND", HttpStatusCode.NotFound);
        }

        return ServiceResult<WritingBookWithLanguageId>.Success(cachedResult);
    }
}
