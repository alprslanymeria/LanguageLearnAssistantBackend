using App.Application.Contracts.Infrastructure.Caching;

namespace App.Application.Features.Practices.CacheKeys;

/// <summary>
/// CACHE KEY DEFINITIONS FOR PRACTICE ENTITY.
/// </summary>
public static class PracticeCacheKeys
{
    public static string Prefix => "practice";
    public static string AllPracticesKey => $"{Prefix}.all";
    public static string PracticeByIdKey => $"{Prefix}.id.{{0}}";
    public static string PracticesByLanguageKey => $"{Prefix}.language.{{0}}";

    public static ICacheKey All(ICacheKeyFactory factory) =>
        factory.Create(_ => null!, AllPracticesKey);

    public static ICacheKey ById(ICacheKeyFactory factory, int id) =>
        factory.Create(_ => null!, PracticeByIdKey, id);

    public static ICacheKey ByLanguage(ICacheKeyFactory factory, string language) =>
        factory.Create(_ => null!, PracticesByLanguageKey, language);
}
