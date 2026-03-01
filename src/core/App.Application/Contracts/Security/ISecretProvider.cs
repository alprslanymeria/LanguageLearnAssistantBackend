namespace App.Application.Contracts.Security;

/// <summary>
/// ABSTRACTION FOR RETRIEVING SECRETS FROM A SECRET MANAGEMENT SERVICE
/// </summary>
public interface ISecretProvider
{
    Task<string> GetSecretAsync(string secretName, CancellationToken cancellationToken = default);
}
