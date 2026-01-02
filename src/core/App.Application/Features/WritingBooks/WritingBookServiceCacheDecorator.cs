using App.Application.Common;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Features.WritingBooks.CacheKeys;
using App.Application.Features.WritingBooks.Dtos;

namespace App.Application.Features.WritingBooks;

/// <summary>
/// CACHE DECORATOR FOR WRITING BOOK SERVICE USING DECORATOR DESIGN PATTERN.
/// </summary>
public class WritingBookServiceCacheDecorator(

    IWritingBookService innerService,
    IStaticCacheManager cacheManager,
    ICacheKeyFactory cacheKeyFactory
    
    ) : IWritingBookService
{
    public async Task<ServiceResult<WritingBookWithLanguageId>> GetWritingBookItemByIdAsync(int id)
    {
        var cacheKey = WritingBookCacheKeys.ById(cacheKeyFactory, id);

        var cachedResult = await cacheManager.GetAsync(cacheKey, async () =>
        {
            var result = await innerService.GetWritingBookItemByIdAsync(id);
            return result.IsSuccess ? result.Data : null;
        });

        return cachedResult is not null
            ? ServiceResult<WritingBookWithLanguageId>.Success(cachedResult)
            : await innerService.GetWritingBookItemByIdAsync(id);
    }

    public async Task<ServiceResult<PagedResult<WritingBookWithTotalCount>>> GetAllWBooksWithPagingAsync(string userId, PagedRequest request)
    {
        return await innerService.GetAllWBooksWithPagingAsync(userId, request);
    }

    public async Task<ServiceResult<List<WritingBookDto>>> GetWBookCreateItemsAsync(string userId, string language, string practice)
    {
        var cacheKey = WritingBookCacheKeys.CreateItems(cacheKeyFactory, userId, language, practice);

        var cachedResult = await cacheManager.GetAsync(cacheKey, async () =>
        {
            var result = await innerService.GetWBookCreateItemsAsync(userId, language, practice);
            return result.IsSuccess ? result.Data : null;
        });

        return cachedResult is not null
            ? ServiceResult<List<WritingBookDto>>.Success(cachedResult)
            : await innerService.GetWBookCreateItemsAsync(userId, language, practice);
    }

    public async Task<ServiceResult> DeleteWBookItemByIdAsync(int id)
    {
        var result = await innerService.DeleteWBookItemByIdAsync(id);

        if (result.IsSuccess)
        {
            await cacheManager.RemoveByPrefixAsync(WritingBookCacheKeys.Prefix);
        }

        return result;
    }

    public async Task<ServiceResult<WritingBookDto>> WritingBookAddAsync(CreateWritingBookRequest request)
    {
        var result = await innerService.WritingBookAddAsync(request);

        if (result.IsSuccess)
        {
            await cacheManager.RemoveByPrefixAsync(WritingBookCacheKeys.Prefix);
        }

        return result;
    }

    public async Task<ServiceResult<WritingBookDto>> WritingBookUpdateAsync(UpdateWritingBookRequest request)
    {
        var result = await innerService.WritingBookUpdateAsync(request);

        if (result.IsSuccess)
        {
            await cacheManager.RemoveByPrefixAsync(WritingBookCacheKeys.Prefix);
        }

        return result;
    }
}
