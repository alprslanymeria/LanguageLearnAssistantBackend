using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.FlashcardCategories.CacheKeys;
using App.Application.Features.FlashcardCategories.Dtos;
using App.Domain.Entities.FlashcardEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Features.FlashcardCategories.Queries.GetFCategoryCreateItems;

/// <summary>
/// HANDLER FOR GET FLASHCARD CATEGORY CREATE ITEMS QUERY.
/// </summary>
public class GetFCategoryCreateItemsQueryHandler(
    IFlashcardCategoryRepository flashcardCategoryRepository,
    ILanguageRepository languageRepository,
    IPracticeRepository practiceRepository,
    IStaticCacheManager cacheManager,
    ICacheKeyFactory cacheKeyFactory,
    IMapper mapper,
    ILogger<GetFCategoryCreateItemsQueryHandler> logger
    ) : IQueryHandler<GetFCategoryCreateItemsQuery, ServiceResult<List<FlashcardCategoryDto>>>
{
    public async Task<ServiceResult<List<FlashcardCategoryDto>>> Handle(
        GetFCategoryCreateItemsQuery request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("GetFCategoryCreateItemsQueryHandler -> FETCHING FLASHCARD CATEGORY CREATE ITEMS FOR USER: {UserId}", request.UserId);

        // CHECK IF LANGUAGE EXISTS
        var languageExists = await languageRepository.ExistsByNameAsync(request.Language);

        if (languageExists is null)
        {
            logger.LogWarning("GetFCategoryCreateItemsQueryHandler -> LANGUAGE NOT FOUND: {Language}", request.Language);
            return ServiceResult<List<FlashcardCategoryDto>>.Fail($"LANGUAGE '{request.Language}' NOT FOUND", HttpStatusCode.NotFound);
        }

        // CHECK IF PRACTICE EXISTS
        var practiceExists = await practiceRepository.ExistsByNameAndLanguageIdAsync(request.Practice, languageExists.Id);

        if (practiceExists is null)
        {
            logger.LogWarning("GetFCategoryCreateItemsQueryHandler -> PRACTICE NOT FOUND: {Practice} FOR LANGUAGE: {Language}", 
                request.Practice, request.Language);
            return ServiceResult<List<FlashcardCategoryDto>>.Fail(
                $"PRACTICE '{request.Practice}' NOT FOUND FOR LANGUAGE '{request.Language}'.", HttpStatusCode.NotFound);
        }

        var cacheKey = FlashcardCategoryCacheKeys.CreateItems(cacheKeyFactory, request.UserId, request.Language, request.Practice);

        var cachedResult = await cacheManager.GetAsync(cacheKey, async () =>
        {
            var flashcardCategories = await flashcardCategoryRepository.GetFCategoryCreateItemsAsync(
                request.UserId, languageExists.Id, practiceExists.Id);

            logger.LogInformation("GetFCategoryCreateItemsQueryHandler -> SUCCESSFULLY FETCHED {Count} CREATE ITEMS", flashcardCategories.Count);

            return mapper.Map<List<FlashcardCategory>, List<FlashcardCategoryDto>>(flashcardCategories);
        });

        return ServiceResult<List<FlashcardCategoryDto>>.Success(cachedResult ?? []);
    }
}
