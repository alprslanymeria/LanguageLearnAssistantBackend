using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Features.ListeningCategories.CacheKeys;
using App.Application.Features.ListeningCategories.Dtos;

namespace App.Application.Features.ListeningCategories.Queries.GetLCategoryCreateItems;

/// <summary>
/// QUERY FOR RETRIEVING CREATE ITEMS FOR DROPDOWN SELECTIONS.
/// </summary>
public record GetLCategoryCreateItemsQuery(
    string UserId,
    string Language,
    string Practice
) : IQuery<ServiceResult<List<ListeningCategoryWithDeckVideos>>>, ICacheableQuery
{
    public ICacheKey GetCacheKey(ICacheKeyFactory keyFactory) => ListeningCategoryCacheKeys.CreateItems(keyFactory, UserId, Language, Practice);
}
