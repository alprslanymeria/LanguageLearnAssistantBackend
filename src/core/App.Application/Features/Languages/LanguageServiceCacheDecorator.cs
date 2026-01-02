using App.Application.Common;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Features.Languages.CacheKeys;
using App.Application.Features.Languages.Dtos;

namespace App.Application.Features.Languages;

/// <summary>
/// CACHE DECORATOR FOR LANGUAGE SERVICE USING DECORATOR DESIGN PATTERN.
/// </summary>
public class LanguageServiceCacheDecorator(

    ILanguageService innerService,
    IStaticCacheManager cacheManager,
    ICacheKeyFactory cacheKeyFactory
    
    ) : ILanguageService
{
    public async Task<ServiceResult<List<LanguageDto>>> GetLanguagesAsync()
    {
        var cacheKey = LanguageCacheKeys.All(cacheKeyFactory);

        var cachedResult = await cacheManager.GetAsync(cacheKey, async () =>
        {
            var result = await innerService.GetLanguagesAsync();
            return result.IsSuccess ? result.Data : null;
        });

        return cachedResult is not null
            ? ServiceResult<List<LanguageDto>>.Success(cachedResult)
            : await innerService.GetLanguagesAsync();
    }
}
