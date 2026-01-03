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

namespace App.Application.Features.FlashcardCategories.Queries.GetFlashcardCategoryById;

/// <summary>
/// HANDLER FOR GET FLASHCARD CATEGORY BY ID QUERY.
/// </summary>
public class GetFlashcardCategoryByIdQueryHandler(
    IFlashcardCategoryRepository flashcardCategoryRepository,
    IStaticCacheManager cacheManager,
    ICacheKeyFactory cacheKeyFactory,
    IMapper mapper,
    ILogger<GetFlashcardCategoryByIdQueryHandler> logger
    ) : IQueryHandler<GetFlashcardCategoryByIdQuery, ServiceResult<FlashcardCategoryWithLanguageId>>
{
    public async Task<ServiceResult<FlashcardCategoryWithLanguageId>> Handle(
        GetFlashcardCategoryByIdQuery request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("GetFlashcardCategoryByIdQueryHandler -> FETCHING FLASHCARD CATEGORY WITH ID: {Id}", request.Id);

        var cacheKey = FlashcardCategoryCacheKeys.ById(cacheKeyFactory, request.Id);

        var cachedResult = await cacheManager.GetAsync(cacheKey, async () =>
        {
            var flashcardCategory = await flashcardCategoryRepository.GetFlashcardCategoryItemByIdAsync(request.Id);

            if (flashcardCategory is null)
            {
                return null;
            }

            logger.LogInformation("GetFlashcardCategoryByIdQueryHandler -> SUCCESSFULLY FETCHED FLASHCARD CATEGORY: {Name}", flashcardCategory.Name);

            return mapper.Map<FlashcardCategory, FlashcardCategoryWithLanguageId>(flashcardCategory);
        });

        if (cachedResult is null)
        {
            logger.LogWarning("GetFlashcardCategoryByIdQueryHandler -> FLASHCARD CATEGORY NOT FOUND WITH ID: {Id}", request.Id);
            return ServiceResult<FlashcardCategoryWithLanguageId>.Fail("FLASHCARD CATEGORY NOT FOUND", HttpStatusCode.NotFound);
        }

        return ServiceResult<FlashcardCategoryWithLanguageId>.Success(cachedResult);
    }
}
