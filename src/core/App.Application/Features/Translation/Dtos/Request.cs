namespace App.Application.Features.Translation.Dtos;

/// <summary>
/// REQUEST DTO FOR TEXT TRANSLATION.
/// </summary>
public sealed record TranslateTextRequest
(
    string SelectedText,
    string Practice,
    string Language
);
