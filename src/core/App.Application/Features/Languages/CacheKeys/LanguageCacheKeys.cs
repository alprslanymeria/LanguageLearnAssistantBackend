using App.Application.Contracts.Infrastructure.Caching;

namespace App.Application.Features.Languages.CacheKeys;

/// <summary>
/// CACHE KEY DEFINITIONS FOR LANGUAGE ENTITY.
/// </summary>
public static class LanguageCacheKeys
{
    public static string Prefix => "language";
    public static string AllLanguagesKey => $"{Prefix}.all";
    public static string LanguageByIdKey => $"{Prefix}.id.{{0}}";

    public static ICacheKey All(ICacheKeyFactory factory) =>
        factory.Create(_ => null!, AllLanguagesKey);

    public static ICacheKey ById(ICacheKeyFactory factory, int id) =>
        factory.Create(_ => null!, LanguageByIdKey, id);
}
