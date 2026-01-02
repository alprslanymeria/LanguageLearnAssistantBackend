using App.Application.Contracts.Infrastructure.Caching;

namespace App.Application.Features.DeckWords.CacheKeys;

/// <summary>
/// CACHE KEY DEFINITIONS FOR DECK WORD ENTITY.
/// </summary>
public static class DeckWordCacheKeys
{
    public static string Prefix => "deckWord";
    public static string AllWordsKey => $"{Prefix}.all.{{0}}.{{1}}";
    public static string WordByIdKey => $"{Prefix}.id.{{0}}";

    public static ICacheKey Paged(ICacheKeyFactory factory, int page, int pageSize) =>
        factory.Create(_ => null!, AllWordsKey, page, pageSize);

    public static ICacheKey ById(ICacheKeyFactory factory, int id) =>
        factory.Create(_ => null!, WordByIdKey, id);
}
