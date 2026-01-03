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

namespace App.Application.Features.WritingBooks.Queries.GetWBookCreateItems;

/// <summary>
/// HANDLER FOR GET WRITING BOOK CREATE ITEMS QUERY.
/// </summary>
public class GetWBookCreateItemsQueryHandler(
    IWritingBookRepository writingBookRepository,
    ILanguageRepository languageRepository,
    IPracticeRepository practiceRepository,
    IStaticCacheManager cacheManager,
    ICacheKeyFactory cacheKeyFactory,
    IMapper mapper,
    ILogger<GetWBookCreateItemsQueryHandler> logger
    ) : IQueryHandler<GetWBookCreateItemsQuery, ServiceResult<List<WritingBookDto>>>
{
    public async Task<ServiceResult<List<WritingBookDto>>> Handle(
        GetWBookCreateItemsQuery request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("GetWBookCreateItemsQueryHandler -> FETCHING WRITING BOOK CREATE ITEMS FOR USER: {UserId}", request.UserId);

        // CHECK IF LANGUAGE EXISTS
        var languageExists = await languageRepository.ExistsByNameAsync(request.Language);

        if (languageExists is null)
        {
            logger.LogWarning("GetWBookCreateItemsQueryHandler -> LANGUAGE NOT FOUND: {Language}", request.Language);
            return ServiceResult<List<WritingBookDto>>.Fail($"LANGUAGE '{request.Language}' NOT FOUND", HttpStatusCode.NotFound);
        }

        // CHECK IF PRACTICE EXISTS
        var practiceExists = await practiceRepository.ExistsByNameAndLanguageIdAsync(request.Practice, languageExists.Id);

        if (practiceExists is null)
        {
            logger.LogWarning("GetWBookCreateItemsQueryHandler -> PRACTICE NOT FOUND: {Practice} FOR LANGUAGE: {Language}", 
                request.Practice, request.Language);
            return ServiceResult<List<WritingBookDto>>.Fail(
                $"PRACTICE '{request.Practice}' NOT FOUND FOR LANGUAGE '{request.Language}'.", HttpStatusCode.NotFound);
        }

        var cacheKey = WritingBookCacheKeys.CreateItems(cacheKeyFactory, request.UserId, request.Language, request.Practice);

        var cachedResult = await cacheManager.GetAsync(cacheKey, async () =>
        {
            var writingBooks = await writingBookRepository.GetWBookCreateItemsAsync(
                request.UserId, languageExists.Id, practiceExists.Id);

            logger.LogInformation("GetWBookCreateItemsQueryHandler -> SUCCESSFULLY FETCHED {Count} CREATE ITEMS", writingBooks.Count);

            return mapper.Map<List<WritingBook>, List<WritingBookDto>>(writingBooks);
        });

        return ServiceResult<List<WritingBookDto>>.Success(cachedResult ?? []);
    }
}
