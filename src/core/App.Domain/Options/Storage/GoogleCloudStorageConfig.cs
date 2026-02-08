namespace App.Domain.Options.Storage;

/// <summary>
/// CONFIGURATION FOR GOOGLE CLOUD STORAGE
/// </summary>
public class GoogleCloudStorageConfig
{
    public const string Key = "GoogleCloudStorage";

    /// <summary>
    /// PATH TO THE SERVICE ACCOUNT JSON CREDENTIAL FILE
    /// </summary>
    public string CredentialFilePath { get; set; } = string.Empty;

    /// <summary>
    /// GOOGLE CLOUD STORAGE BUCKET NAME
    /// </summary>
    public string BucketName { get; set; } = string.Empty;
}
