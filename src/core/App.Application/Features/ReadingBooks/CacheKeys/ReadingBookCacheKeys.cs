using App.Application.Contracts.Infrastructure.Caching;

namespace App.Application.Features.ReadingBooks.CacheKeys;

/// <summary>
/// CACHE KEY DEFINITIONS FOR READING BOOK ENTITY.
/// </summary>
public static class ReadingBookCacheKeys
{
    public static string Prefix => "readingBook";
    public static string AllBooksKey => $"{Prefix}.all.{{0}}.{{1}}";
    public static string BookByIdKey => $"{Prefix}.id.{{0}}";
    public static string CreateItemsKey => $"{Prefix}.createItems.{{0}}.{{1}}.{{2}}";

    public static ICacheKey Paged(ICacheKeyFactory factory, int page, int pageSize) =>
        factory.Create(_ => null!, AllBooksKey, page, pageSize);

    public static ICacheKey ById(ICacheKeyFactory factory, int id) =>
        factory.Create(_ => null!, BookByIdKey, id);

    public static ICacheKey CreateItems(ICacheKeyFactory factory, string userId, string language, string practice) =>
        factory.Create(_ => null!, CreateItemsKey, userId, language, practice);
}
