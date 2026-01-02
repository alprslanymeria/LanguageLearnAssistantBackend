using App.Application.Common;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Features.DeckWords.CacheKeys;
using App.Application.Features.DeckWords.Dtos;

namespace App.Application.Features.DeckWords;

/// <summary>
/// CACHE DECORATOR FOR DECK WORD SERVICE USING DECORATOR DESIGN PATTERN.
/// </summary>
public class DeckWordServiceCacheDecorator(

    IDeckWordService innerService,
    IStaticCacheManager cacheManager,
    ICacheKeyFactory cacheKeyFactory
    
    ) : IDeckWordService
{
    public async Task<ServiceResult<DeckWordWithLanguageId>> GetDeckWordItemByIdAsync(int id)
    {
        var cacheKey = DeckWordCacheKeys.ById(cacheKeyFactory, id);

        var cachedResult = await cacheManager.GetAsync(cacheKey, async () =>
        {
            var result = await innerService.GetDeckWordItemByIdAsync(id);
            return result.IsSuccess ? result.Data : null;
        });

        return cachedResult is not null
            ? ServiceResult<DeckWordWithLanguageId>.Success(cachedResult)
            : await innerService.GetDeckWordItemByIdAsync(id);
    }

    public async Task<ServiceResult<PagedResult<DeckWordWithTotalCount>>> GetAllDWordsWithPagingAsync(int categoryId, PagedRequest request)
    {
        return await innerService.GetAllDWordsWithPagingAsync(categoryId, request);
    }

    public async Task<ServiceResult> DeleteDWordItemByIdAsync(int id)
    {
        var result = await innerService.DeleteDWordItemByIdAsync(id);

        if (result.IsSuccess)
        {
            await cacheManager.RemoveByPrefixAsync(DeckWordCacheKeys.Prefix);
        }

        return result;
    }

    public async Task<ServiceResult<DeckWordDto>> DeckWordAddAsync(CreateDeckWordRequest request)
    {
        var result = await innerService.DeckWordAddAsync(request);

        if (result.IsSuccess)
        {
            await cacheManager.RemoveByPrefixAsync(DeckWordCacheKeys.Prefix);
        }

        return result;
    }

    public async Task<ServiceResult<DeckWordDto>> DeckWordUpdateAsync(UpdateDeckWordRequest request)
    {
        var result = await innerService.DeckWordUpdateAsync(request);

        if (result.IsSuccess)
        {
            await cacheManager.RemoveByPrefixAsync(DeckWordCacheKeys.Prefix);
        }

        return result;
    }
}
