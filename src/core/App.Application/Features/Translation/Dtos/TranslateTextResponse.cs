namespace App.Application.Features.Translation.Dtos;

/// <summary>
/// RESPONSE DTO FOR TEXT TRANSLATION.
/// </summary>
public sealed record TranslateTextResponse
{
    /// <summary>
    /// THE ORIGINAL TEXT.
    /// </summary>
    public required string OriginalText { get; init; }

    /// <summary>
    /// THE TRANSLATED TEXT.
    /// </summary>
    public required string TranslatedText { get; init; }

    /// <summary>
    /// THE TARGET LANGUAGE CODE.
    /// </summary>
    public required string TargetLanguage { get; init; }
}
