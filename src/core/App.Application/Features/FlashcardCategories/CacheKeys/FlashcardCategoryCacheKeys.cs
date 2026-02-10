using App.Application.Contracts.Infrastructure.Caching;

namespace App.Application.Features.FlashcardCategories.CacheKeys;

/// <summary>
/// CACHE KEY DEFINITIONS FOR FLASHCARD CATEGORY ENTITY.
/// </summary>
public static class FlashcardCategoryCacheKeys
{
    public static string Prefix => "flashcardCategory";
    private static string AllCategoriesKey => $"{Prefix}.all.{{0}}.{{1}}";
    private static string CategoryByIdKey => $"{Prefix}.id.{{0}}";
    private static string CreateItemsKey => $"{Prefix}.createItems.{{0}}.{{1}}.{{2}}";

    public static ICacheKey Paged(ICacheKeyFactory factory, int page, int pageSize) =>
        factory.Create(x => x, AllCategoriesKey, page, pageSize);

    public static ICacheKey ById(ICacheKeyFactory factory, int id) =>
        factory.Create(x => x, CategoryByIdKey, id);

    public static ICacheKey CreateItems(ICacheKeyFactory factory, string userId, string language, string practice) =>
        factory.Create(x => x, CreateItemsKey, userId, language, practice);
}
