using App.Application.Contracts.Infrastructure.Caching;

namespace App.Application.Features.FlashcardCategories.CacheKeys;

/// <summary>
/// CACHE KEY DEFINITIONS FOR FLASHCARD CATEGORY ENTITY.
/// </summary>
public static class FlashcardCategoryCacheKeys
{
    public static string Prefix => "flashcardCategory";
    public static string AllCategoriesKey => $"{Prefix}.all.{{0}}.{{1}}";
    public static string CategoryByIdKey => $"{Prefix}.id.{{0}}";
    public static string CreateItemsKey => $"{Prefix}.createItems.{{0}}.{{1}}.{{2}}";

    public static ICacheKey Paged(ICacheKeyFactory factory, int page, int pageSize) =>
        factory.Create(_ => null!, AllCategoriesKey, page, pageSize);

    public static ICacheKey ById(ICacheKeyFactory factory, int id) =>
        factory.Create(_ => null!, CategoryByIdKey, id);

    public static ICacheKey CreateItems(ICacheKeyFactory factory, string userId, string language, string practice) =>
        factory.Create(_ => null!, CreateItemsKey, userId, language, practice);
}
