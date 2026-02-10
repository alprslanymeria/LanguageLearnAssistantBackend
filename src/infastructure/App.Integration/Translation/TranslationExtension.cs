using App.Application.Contracts.Infrastructure.Translation;
using App.Domain.Options.Translation;
using Google.Cloud.Translation.V2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Integration.Translation;

/// <summary>
/// EXTENSION FOR REGISTERING TRANSLATION SERVICES WITH DEPENDENCY INJECTION.
/// </summary>
public static class TranslationExtension
{
    public static IServiceCollection AddTranslationServicesExt(this IServiceCollection services, IConfiguration configuration)
    {
        // LOAD TRANSLATION CONFIGURATION AND VALIDATE
        var translationConfig = configuration
            .GetRequiredSection(TranslationConfig.Key)
            .Get<TranslationConfig>()
            ?? throw new InvalidOperationException("Translation configuration is missing.");

        // CONFIGURATION BINDINGS
        services.Configure<TranslationConfig>(configuration.GetSection(TranslationConfig.Key));
        services.Configure<GoogleTranslateConfig>(configuration.GetSection(GoogleTranslateConfig.Key));

        // COMMON TRANSLATION SERVICES

        // REGISTER BASED ON CONFIGURATION
        switch (translationConfig.TranslationType)
        {
            case TranslationType.Google:
                AddGoogleProvider(services, configuration);
                break;

            default:
                throw new NotSupportedException($"Translation type '{translationConfig.TranslationType}' is not supported.");
        }

        return services;
    }

    private static void AddGoogleProvider(IServiceCollection services, IConfiguration configuration)
    {

        services.AddSingleton<TranslationClient>(_ => TranslationClient.Create());

        services.AddSingleton<ITranslationProvider, GoogleTranslationProvider>();
    }
}
