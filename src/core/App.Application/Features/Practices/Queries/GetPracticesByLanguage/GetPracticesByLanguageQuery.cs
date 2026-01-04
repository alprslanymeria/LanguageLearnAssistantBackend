using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Features.Practices.CacheKeys;
using App.Application.Features.Practices.Dtos;

namespace App.Application.Features.Practices.Queries.GetPracticesByLanguage;

/// <summary>
/// QUERY FOR RETRIEVING PRACTICES BY SPECIFIED LANGUAGE.
/// </summary>
public record GetPracticesByLanguageQuery(string Language) : IQuery<ServiceResult<List<PracticeDto>>> , ICacheableQuery
{
    public ICacheKey GetCacheKey(ICacheKeyFactory keyFactory) => PracticeCacheKeys.ByLanguage(keyFactory, Language);
}
