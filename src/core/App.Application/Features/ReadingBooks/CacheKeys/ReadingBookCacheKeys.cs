using App.Application.Contracts.Infrastructure.Caching;

namespace App.Application.Features.ReadingBooks.CacheKeys;

/// <summary>
/// CACHE KEY DEFINITIONS FOR READING BOOK ENTITY.
/// </summary>
public static class ReadingBookCacheKeys
{
    public static string Prefix => "readingBook";
    private static string AllBooksKey => $"{Prefix}.all.{{0}}.{{1}}";
    private static string BookByIdKey => $"{Prefix}.id.{{0}}";
    private static string CreateItemsKey => $"{Prefix}.createItems.{{0}}.{{1}}.{{2}}";

    public static ICacheKey Paged(ICacheKeyFactory factory, int page, int pageSize) =>
        factory.Create(x => x, AllBooksKey, page, pageSize);

    public static ICacheKey ById(ICacheKeyFactory factory, int id) =>
        factory.Create(x => x, BookByIdKey, id);

    public static ICacheKey CreateItems(ICacheKeyFactory factory, string userId, string language, string practice) =>
        factory.Create(x => x, CreateItemsKey, userId, language, practice);
}
