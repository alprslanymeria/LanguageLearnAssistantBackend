using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.Languages.CacheKeys;
using App.Application.Features.Languages.Dtos;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.Languages.Queries.GetLanguages;

/// <summary>
/// HANDLER FOR GET LANGUAGES QUERY.
/// </summary>
public class GetLanguagesQueryHandler(
    ILanguageRepository languageRepository,
    IStaticCacheManager cacheManager,
    ICacheKeyFactory cacheKeyFactory,
    IMapper mapper,
    ILogger<GetLanguagesQueryHandler> logger
    ) : IQueryHandler<GetLanguagesQuery, ServiceResult<List<LanguageDto>>>
{
    public async Task<ServiceResult<List<LanguageDto>>> Handle(
        GetLanguagesQuery request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("GetLanguagesQueryHandler -> FETCHING ALL LANGUAGES");

        var cacheKey = LanguageCacheKeys.All(cacheKeyFactory);

        var cachedResult = await cacheManager.GetAsync(cacheKey, async () =>
        {
            var languages = await languageRepository.GetLanguagesAsync();

            if (languages.Count == 0)
            {
                logger.LogInformation("GetLanguagesQueryHandler -> LANGUAGES IS EMPTY");
                return new List<LanguageDto>();
            }

            logger.LogInformation("GetLanguagesQueryHandler -> SUCCESSFULLY FETCHED {Count} LANGUAGES", languages.Count);

            return mapper.Map<List<LanguageDto>>(languages);
        });

        return ServiceResult<List<LanguageDto>>.Success(cachedResult ?? []);
    }
}
