using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace App.Storage.google;

/// <summary>
/// FACTORY IMPLEMENTATION FOR CREATING GOOGLE CLOUD STORAGE CLIENTS
/// </summary>
public class GoogleCloudStorageClientFactory(GoogleCredential credential) : IGoogleCloudStorageClientFactory
{

    // IMPLEMENTATION OF IGoogleCloudStorageClientFactory
    public StorageClient CreateStorageClient() => StorageClient.Create(credential);

    public UrlSigner CreateUrlSigner()
    {
        return credential.UnderlyingCredential switch
        {
            ServiceAccountCredential serviceAccountCredential => UrlSigner.FromCredential(serviceAccountCredential),
            _ => UrlSigner.FromCredential(credential)
        };
    }
}
