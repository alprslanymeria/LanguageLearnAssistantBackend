using App.Application.Contracts.Security;
using App.Domain.Options;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace App.Security;

public static class SecurityExtension
{

    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSecretManagerServices(configuration);
        return services;
    }

    private static IServiceCollection AddSecretManagerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SecretManagerOption>(configuration.GetSection(SecretManagerOption.Key));
        services.AddSingleton<ISecretProvider, GoogleSecretManagerProvider>();

        // RESOLVE GOOGLE CREDENTIAL FROM SECRET MANAGER AT STARTUP (SINGLETON)
        services.AddSingleton(sp =>
        {
            var secretProvider = sp.GetRequiredService<ISecretProvider>();
            var options = sp.GetRequiredService<IOptions<SecretManagerOption>>().Value;
            var json = secretProvider.GetSecretAsync(options.ServiceAccountSecretName).GetAwaiter().GetResult();
            return CredentialFactory.FromJson<ServiceAccountCredential>(json).ToGoogleCredential();
        });

        return services;
    }
}
