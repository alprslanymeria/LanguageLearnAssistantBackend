using App.Application.Contracts.Infrastructure.Caching;

namespace App.Application.Features.ListeningCategories.CacheKeys;

/// <summary>
/// CACHE KEY DEFINITIONS FOR LISTENING CATEGORY ENTITY.
/// </summary>
public static class ListeningCategoryCacheKeys
{
    public static string Prefix => "listeningCategory";
    private static string CreateItemsKey => $"{Prefix}.createItems.{{0}}.{{1}}.{{2}}";

    public static ICacheKey CreateItems(ICacheKeyFactory factory, string userId, string language, string practice) =>
        factory.Create(_ => null!, CreateItemsKey, userId, language, practice);
}
