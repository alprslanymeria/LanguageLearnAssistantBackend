namespace App.Application.Features.Translation.Dtos;

/// <summary>
/// RESPONSE DTO FOR TEXT TRANSLATION.
/// </summary>
public record TranslateTextResponse
(
    string OriginalText,
    string TranslatedText,
    string TargetLanguageCode
);
