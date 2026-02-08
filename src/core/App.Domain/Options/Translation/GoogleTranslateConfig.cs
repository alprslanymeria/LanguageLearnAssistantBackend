namespace App.Domain.Options.Translation;

/// <summary>
/// CONFIGURATION OPTIONS FOR GOOGLE CLOUD TRANSLATE API.
/// </summary>
public class GoogleTranslateConfig
{
    public const string Key = "GoogleTranslate";

    /// <summary>
    /// PATH TO THE GOOGLE APPLICATION CREDENTIALS JSON FILE.
    /// </summary>
    public string CredentialsPath { get; set; } = null!;
}
