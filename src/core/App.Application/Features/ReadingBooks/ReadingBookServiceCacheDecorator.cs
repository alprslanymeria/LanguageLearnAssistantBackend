using App.Application.Common;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Features.ReadingBooks.CacheKeys;
using App.Application.Features.ReadingBooks.Dtos;

namespace App.Application.Features.ReadingBooks;

/// <summary>
/// CACHE DECORATOR FOR READING BOOK SERVICE USING DECORATOR DESIGN PATTERN.
/// </summary>
public class ReadingBookServiceCacheDecorator(

    IReadingBookService innerService,
    IStaticCacheManager cacheManager,
    ICacheKeyFactory cacheKeyFactory
    
    ) : IReadingBookService
{
    public async Task<ServiceResult<ReadingBookWithLanguageId>> GetReadingBookItemByIdAsync(int id)
    {
        var cacheKey = ReadingBookCacheKeys.ById(cacheKeyFactory, id);

        var cachedResult = await cacheManager.GetAsync(cacheKey, async () =>
        {
            var result = await innerService.GetReadingBookItemByIdAsync(id);
            return result.IsSuccess ? result.Data : null;
        });

        return cachedResult is not null
            ? ServiceResult<ReadingBookWithLanguageId>.Success(cachedResult)
            : await innerService.GetReadingBookItemByIdAsync(id);
    }

    public async Task<ServiceResult<PagedResult<ReadingBookWithTotalCount>>> GetAllRBooksWithPagingAsync(string userId, PagedRequest request)
    {
        // PAGINATION RESULTS ARE NOT CACHED DUE TO COMPLEXITY
        return await innerService.GetAllRBooksWithPagingAsync(userId, request);
    }

    public async Task<ServiceResult<List<ReadingBookDto>>> GetRBookCreateItemsAsync(string userId, string language, string practice)
    {
        var cacheKey = ReadingBookCacheKeys.CreateItems(cacheKeyFactory, userId, language, practice);

        var cachedResult = await cacheManager.GetAsync(cacheKey, async () =>
        {
            var result = await innerService.GetRBookCreateItemsAsync(userId, language, practice);
            return result.IsSuccess ? result.Data : null;
        });

        return cachedResult is not null
            ? ServiceResult<List<ReadingBookDto>>.Success(cachedResult)
            : await innerService.GetRBookCreateItemsAsync(userId, language, practice);
    }

    public async Task<ServiceResult> DeleteRBookItemByIdAsync(int id)
    {
        var result = await innerService.DeleteRBookItemByIdAsync(id);

        if (result.IsSuccess)
        {
            await cacheManager.RemoveByPrefixAsync(ReadingBookCacheKeys.Prefix);
        }

        return result;
    }

    public async Task<ServiceResult<ReadingBookDto>> ReadingBookAddAsync(CreateReadingBookRequest request)
    {
        var result = await innerService.ReadingBookAddAsync(request);

        if (result.IsSuccess)
        {
            await cacheManager.RemoveByPrefixAsync(ReadingBookCacheKeys.Prefix);
        }

        return result;
    }

    public async Task<ServiceResult<ReadingBookDto>> ReadingBookUpdateAsync(UpdateReadingBookRequest request)
    {
        var result = await innerService.ReadingBookUpdateAsync(request);

        if (result.IsSuccess)
        {
            await cacheManager.RemoveByPrefixAsync(ReadingBookCacheKeys.Prefix);
        }

        return result;
    }
}
