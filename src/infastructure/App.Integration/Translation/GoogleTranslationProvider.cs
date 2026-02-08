using App.Application.Contracts.Infrastructure.Translation;
using Google.Cloud.Translation.V2;
using Microsoft.Extensions.Logging;

namespace App.Integration.Translation;

/// <summary>
/// GOOGLE CLOUD TRANSLATE IMPLEMENTATION OF TRANSLATION PROVIDER.
/// </summary>
public class GoogleTranslationProvider(

    TranslationClient client,
    ILogger<GoogleTranslationProvider> logger

    ) : ITranslationProvider

{
    public async Task<string> TranslateAsync(string text, string targetLanguageCode, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("GoogleTranslationProvider:TranslateAsync -> TRANSLATING TEXT TO {TargetLanguage}", targetLanguageCode);

        var response = await client.TranslateTextAsync(text, targetLanguageCode, cancellationToken: cancellationToken);

        logger.LogInformation("GoogleTranslationProvider:TranslateAsync -> TRANSLATION SUCCESSFUL");

        return response.TranslatedText;
    }
}
