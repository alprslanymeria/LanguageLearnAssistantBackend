namespace App.Application.Features.Translation.Dtos;

/// <summary>
/// REQUEST DTO FOR TEXT TRANSLATION.
/// </summary>
public sealed record TranslateTextRequest
{
    /// <summary>
    /// THE TEXT TO TRANSLATE.
    /// </summary>
    public required string SelectedText { get; init; }

    /// <summary>
    /// THE PRACTICE TYPE (READING, LISTENING, WRITING).
    /// </summary>
    public required string Practice { get; init; }

    /// <summary>
    /// THE LANGUAGE NAME (USED WHEN PRACTICE IS WRITING).
    /// </summary>
    public string? Language { get; init; }
}
