using App.Application.Common;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Features.FlashcardCategories.CacheKeys;
using App.Application.Features.FlashcardCategories.Dtos;

namespace App.Application.Features.FlashcardCategories;

/// <summary>
/// CACHE DECORATOR FOR FLASHCARD CATEGORY SERVICE USING DECORATOR DESIGN PATTERN.
/// </summary>
public class FlashcardCategoryServiceCacheDecorator(

    IFlashcardCategoryService innerService,
    IStaticCacheManager cacheManager,
    ICacheKeyFactory cacheKeyFactory
    
    ) : IFlashcardCategoryService
{
    public async Task<ServiceResult<FlashcardCategoryWithLanguageId>> GetFlashcardCategoryItemByIdAsync(int id)
    {
        var cacheKey = FlashcardCategoryCacheKeys.ById(cacheKeyFactory, id);

        var cachedResult = await cacheManager.GetAsync(cacheKey, async () =>
        {
            var result = await innerService.GetFlashcardCategoryItemByIdAsync(id);
            return result.IsSuccess ? result.Data : null;
        });

        return cachedResult is not null
            ? ServiceResult<FlashcardCategoryWithLanguageId>.Success(cachedResult)
            : await innerService.GetFlashcardCategoryItemByIdAsync(id);
    }

    public async Task<ServiceResult<PagedResult<FlashcardCategoryWithTotalCount>>> GetAllFCategoriesWithPagingAsync(string userId, PagedRequest request)
    {
        return await innerService.GetAllFCategoriesWithPagingAsync(userId, request);
    }

    public async Task<ServiceResult<List<FlashcardCategoryDto>>> GetFCategoryCreateItemsAsync(string userId, string language, string practice)
    {
        var cacheKey = FlashcardCategoryCacheKeys.CreateItems(cacheKeyFactory, userId, language, practice);

        var cachedResult = await cacheManager.GetAsync(cacheKey, async () =>
        {
            var result = await innerService.GetFCategoryCreateItemsAsync(userId, language, practice);
            return result.IsSuccess ? result.Data : null;
        });

        return cachedResult is not null
            ? ServiceResult<List<FlashcardCategoryDto>>.Success(cachedResult)
            : await innerService.GetFCategoryCreateItemsAsync(userId, language, practice);
    }

    public async Task<ServiceResult> DeleteFCategoryItemByIdAsync(int id)
    {
        var result = await innerService.DeleteFCategoryItemByIdAsync(id);

        if (result.IsSuccess)
        {
            await cacheManager.RemoveByPrefixAsync(FlashcardCategoryCacheKeys.Prefix);
        }

        return result;
    }

    public async Task<ServiceResult<FlashcardCategoryDto>> FlashcardCategoryAddAsync(CreateFlashcardCategoryRequest request)
    {
        var result = await innerService.FlashcardCategoryAddAsync(request);

        if (result.IsSuccess)
        {
            await cacheManager.RemoveByPrefixAsync(FlashcardCategoryCacheKeys.Prefix);
        }

        return result;
    }

    public async Task<ServiceResult<FlashcardCategoryDto>> FlashcardCategoryUpdateAsync(UpdateFlashcardCategoryRequest request)
    {
        var result = await innerService.FlashcardCategoryUpdateAsync(request);

        if (result.IsSuccess)
        {
            await cacheManager.RemoveByPrefixAsync(FlashcardCategoryCacheKeys.Prefix);
        }

        return result;
    }
}
