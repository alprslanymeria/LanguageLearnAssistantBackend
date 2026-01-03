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

namespace App.Application.Features.ReadingBooks.Queries.GetRBookCreateItems;

/// <summary>
/// HANDLER FOR GET READING BOOK CREATE ITEMS QUERY.
/// </summary>
public class GetRBookCreateItemsQueryHandler(
    IReadingBookRepository readingBookRepository,
    ILanguageRepository languageRepository,
    IPracticeRepository practiceRepository,
    IStaticCacheManager cacheManager,
    ICacheKeyFactory cacheKeyFactory,
    IMapper mapper,
    ILogger<GetRBookCreateItemsQueryHandler> logger
    ) : IQueryHandler<GetRBookCreateItemsQuery, ServiceResult<List<ReadingBookDto>>>
{
    public async Task<ServiceResult<List<ReadingBookDto>>> Handle(
        GetRBookCreateItemsQuery request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("GetRBookCreateItemsQueryHandler -> FETCHING READING BOOK CREATE ITEMS FOR USER: {UserId}", request.UserId);

        // CHECK IF LANGUAGE EXISTS
        var languageExists = await languageRepository.ExistsByNameAsync(request.Language);

        if (languageExists is null)
        {
            logger.LogWarning("GetRBookCreateItemsQueryHandler -> LANGUAGE NOT FOUND: {Language}", request.Language);
            return ServiceResult<List<ReadingBookDto>>.Fail($"LANGUAGE '{request.Language}' NOT FOUND", HttpStatusCode.NotFound);
        }

        // CHECK IF PRACTICE EXISTS
        var practiceExists = await practiceRepository.ExistsByNameAndLanguageIdAsync(request.Practice, languageExists.Id);

        if (practiceExists is null)
        {
            logger.LogWarning("GetRBookCreateItemsQueryHandler -> PRACTICE NOT FOUND: {Practice} FOR LANGUAGE: {Language}", 
                request.Practice, request.Language);
            return ServiceResult<List<ReadingBookDto>>.Fail(
                $"PRACTICE '{request.Practice}' NOT FOUND FOR LANGUAGE '{request.Language}'.", HttpStatusCode.NotFound);
        }

        var cacheKey = ReadingBookCacheKeys.CreateItems(cacheKeyFactory, request.UserId, request.Language, request.Practice);

        var cachedResult = await cacheManager.GetAsync(cacheKey, async () =>
        {
            var readingBooks = await readingBookRepository.GetRBookCreateItemsAsync(
                request.UserId, languageExists.Id, practiceExists.Id);

            logger.LogInformation("GetRBookCreateItemsQueryHandler -> SUCCESSFULLY FETCHED {Count} CREATE ITEMS", readingBooks.Count);

            return mapper.Map<List<ReadingBook>, List<ReadingBookDto>>(readingBooks);
        });

        return ServiceResult<List<ReadingBookDto>>.Success(cachedResult ?? []);
    }
}
