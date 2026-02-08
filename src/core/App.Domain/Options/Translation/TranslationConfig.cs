namespace App.Domain.Options.Translation;

/// <summary>
/// CONFIGURATION OPTIONS FOR TRANSLATION SERVICE.
/// </summary>
public class TranslationConfig
{
    public const string Key = "TranslationConfig";

    public TranslationType TranslationType { get; set; } = TranslationType.Google;
}
