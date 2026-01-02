namespace App.Application.Contracts.Infrastructure.Translation;

/// <summary>
/// INTERFACE FOR TRANSLATION PROVIDER (E.G., GOOGLE CLOUD TRANSLATE).
/// </summary>
public interface ITranslationProvider
{
    /// <summary>
    /// TRANSLATES TEXT TO THE TARGET LANGUAGE.
    /// </summary>
    /// <param name="text">THE TEXT TO TRANSLATE.</param>
    /// <param name="targetLanguageCode">THE TARGET LANGUAGE CODE (E.G., "en", "tr", "de").</param>
    /// <param name="cancellationToken">CANCELLATION TOKEN.</param>
    /// <returns>THE TRANSLATED TEXT.</returns>
    Task<string> TranslateAsync(string text, string targetLanguageCode, CancellationToken cancellationToken = default);
}
