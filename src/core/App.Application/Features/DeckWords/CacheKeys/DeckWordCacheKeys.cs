using App.Application.Contracts.Infrastructure.Caching;

namespace App.Application.Features.DeckWords.CacheKeys;

/// <summary>
/// CACHE KEY DEFINITIONS FOR DECK WORD ENTITY.
/// </summary>
public static class DeckWordCacheKeys
{
    public static string Prefix => "deckWord";
    private static string AllWordsKey => $"{Prefix}.all.{{0}}.{{1}}";
    private static string WordByIdKey => $"{Prefix}.id.{{0}}";

    public static ICacheKey Paged(ICacheKeyFactory factory, int page, int pageSize) =>
        factory.Create(x => x, AllWordsKey, page, pageSize);

    public static ICacheKey ById(ICacheKeyFactory factory, int id) =>
        factory.Create(x => x, WordByIdKey, id);
}
