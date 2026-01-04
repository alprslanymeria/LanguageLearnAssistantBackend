using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Features.FlashcardCategories.CacheKeys;
using App.Application.Features.FlashcardCategories.Dtos;

namespace App.Application.Features.FlashcardCategories.Queries.GetFCategoryCreateItems;

/// <summary>
/// QUERY FOR RETRIEVING CREATE ITEMS FOR DROPDOWN SELECTIONS.
/// </summary>
public record GetFCategoryCreateItemsQuery(
    string UserId,
    string Language,
    string Practice
) : IQuery<ServiceResult<List<FlashcardCategoryDto>>>, ICacheableQuery
{
    public ICacheKey GetCacheKey(ICacheKeyFactory keyFactory) => FlashcardCategoryCacheKeys.CreateItems(keyFactory, UserId, Language, Practice);
}
