using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Features.ReadingBooks.CacheKeys;
using App.Application.Features.ReadingBooks.Dtos;

namespace App.Application.Features.ReadingBooks.Queries.GetRBookCreateItems;

/// <summary>
/// QUERY FOR RETRIEVING CREATE ITEMS FOR DROPDOWN SELECTIONS.
/// </summary>
public record GetRBookCreateItemsQuery(
    string UserId,
    string Language,
    string Practice) : IQuery<ServiceResult<List<ReadingBookDto>>>, ICacheableQuery
{
    public ICacheKey GetCacheKey(ICacheKeyFactory keyFactory) => ReadingBookCacheKeys.CreateItems(keyFactory, UserId, Language, Practice);
}
