using App.Application.Common;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Features.Practices.CacheKeys;
using App.Application.Features.Practices.Dtos;

namespace App.Application.Features.Practices;

/// <summary>
/// CACHE DECORATOR FOR PRACTICE SERVICE USING DECORATOR DESIGN PATTERN.
/// </summary>
public class PracticeServiceCacheDecorator(
    IPracticeService innerService,
    IStaticCacheManager cacheManager,
    ICacheKeyFactory cacheKeyFactory) : IPracticeService
{
    public async Task<ServiceResult<List<PracticeDto>>> GetPracticesByLanguageAsync(string language)
    {
        var cacheKey = PracticeCacheKeys.ByLanguage(cacheKeyFactory, language);

        var cachedResult = await cacheManager.GetAsync(cacheKey, async () =>
        {
            var result = await innerService.GetPracticesByLanguageAsync(language);
            return result.IsSuccess ? result.Data : null;
        });

        return cachedResult is not null
            ? ServiceResult<List<PracticeDto>>.Success(cachedResult)
            : await innerService.GetPracticesByLanguageAsync(language);
    }
}
