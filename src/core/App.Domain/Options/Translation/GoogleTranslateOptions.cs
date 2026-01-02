namespace App.Domain.Options.Translation;

/// <summary>
/// CONFIGURATION OPTIONS FOR GOOGLE CLOUD TRANSLATE API.
/// </summary>
public class GoogleTranslateOptions
{
    public const string Key = "GoogleTranslate";

    /// <summary>
    /// PATH TO THE GOOGLE APPLICATION CREDENTIALS JSON FILE.
    /// </summary>
    public string CredentialsPath { get; set; } = null!;

    /// <summary>
    /// GOOGLE CLOUD PROJECT ID (OPTIONAL, CAN BE INFERRED FROM CREDENTIALS).
    /// </summary>
    public string? ProjectId { get; set; }
}
