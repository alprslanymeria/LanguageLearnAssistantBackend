using App.Application.Contracts.Infrastructure.Caching;

namespace App.Application.Features.Practices.CacheKeys;

/// <summary>
/// CACHE KEY DEFINITIONS FOR PRACTICE ENTITY.
/// </summary>
public static class PracticeCacheKeys
{
    public static string Prefix => "practice";
    private static string AllPracticesKey => $"{Prefix}.all";
    private static string PracticeByIdKey => $"{Prefix}.id.{{0}}";
    private static string PracticesByLanguageKey => $"{Prefix}.language.{{0}}";

    public static ICacheKey All(ICacheKeyFactory factory) =>
        factory.Create(x => x, AllPracticesKey);

    public static ICacheKey ById(ICacheKeyFactory factory, int id) =>
        factory.Create(x => x, PracticeByIdKey, id);

    public static ICacheKey ByLanguage(ICacheKeyFactory factory, string language) =>
        factory.Create(x => x, PracticesByLanguageKey, language);
}
