using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Features.Languages.CacheKeys;
using App.Application.Features.Languages.Dtos;

namespace App.Application.Features.Languages.Queries.GetLanguages;

/// <summary>
/// QUERY FOR RETRIEVING ALL LANGUAGES WITH PRACTICE COUNTS.
/// </summary>
public record GetLanguagesQuery : IQuery<ServiceResult<List<LanguageDto>>>, ICacheableQuery
{
    public ICacheKey GetCacheKey(ICacheKeyFactory keyFactory) => LanguageCacheKeys.All(keyFactory);
};
