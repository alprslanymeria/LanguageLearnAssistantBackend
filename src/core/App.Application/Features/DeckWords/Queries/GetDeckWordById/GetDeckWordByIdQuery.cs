using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Features.DeckWords.CacheKeys;
using App.Application.Features.DeckWords.Dtos;

namespace App.Application.Features.DeckWords.Queries.GetDeckWordById;

/// <summary>
/// QUERY FOR RETRIEVING A DECK WORD BY ID.
/// </summary>
public record GetDeckWordByIdQuery(int Id) : IQuery<ServiceResult<DeckWordWithLanguageId>>, ICacheableQuery
{
    public ICacheKey GetCacheKey(ICacheKeyFactory keyFactory) => DeckWordCacheKeys.ById(keyFactory, Id);
}
