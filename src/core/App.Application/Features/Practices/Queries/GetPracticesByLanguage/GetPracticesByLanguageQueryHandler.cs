using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.Practices.CacheKeys;
using App.Application.Features.Practices.Dtos;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Features.Practices.Queries.GetPracticesByLanguage;

/// <summary>
/// HANDLER FOR GET PRACTICES BY LANGUAGE QUERY.
/// </summary>
public class GetPracticesByLanguageQueryHandler(
    IPracticeRepository practiceRepository,
    ILanguageRepository languageRepository,
    IStaticCacheManager cacheManager,
    ICacheKeyFactory cacheKeyFactory,
    IMapper mapper,
    ILogger<GetPracticesByLanguageQueryHandler> logger
    ) : IQueryHandler<GetPracticesByLanguageQuery, ServiceResult<List<PracticeDto>>>
{
    public async Task<ServiceResult<List<PracticeDto>>> Handle(
        GetPracticesByLanguageQuery request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("GetPracticesByLanguageQueryHandler -> FETCHING PRACTICES FOR LANGUAGE: {Language}", request.Language);

        // CHECK IF LANGUAGE EXISTS
        var languageExists = await languageRepository.ExistsByNameAsync(request.Language);

        if (languageExists is null)
        {
            logger.LogWarning("GetPracticesByLanguageQueryHandler -> LANGUAGE NOT FOUND: {Language}", request.Language);
            return ServiceResult<List<PracticeDto>>.Fail($"LANGUAGE '{request.Language}' NOT FOUND.", HttpStatusCode.NotFound);
        }

        var cacheKey = PracticeCacheKeys.ByLanguage(cacheKeyFactory, request.Language);

        var cachedResult = await cacheManager.GetAsync(cacheKey, async () =>
        {
            var practices = await practiceRepository.GetPracticesByLanguageAsync(request.Language);

            logger.LogInformation("GetPracticesByLanguageQueryHandler -> SUCCESSFULLY FETCHED {Count} PRACTICES FOR LANGUAGE: {Language}", 
                practices.Count, request.Language);

            return mapper.Map<List<PracticeDto>>(practices);
        });

        return ServiceResult<List<PracticeDto>>.Success(cachedResult ?? []);
    }
}
