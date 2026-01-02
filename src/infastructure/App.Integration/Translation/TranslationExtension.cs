using App.Application.Contracts.Infrastructure.Translation;
using App.Application.Features.Translation;
using App.Domain.Options.Storage;
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

        // GOOGLE CLOUD TRANSLATE CLIENT
        var googleOptions = configuration.GetSection(GoogleTranslateOptions.Key).Get<GoogleTranslateOptions>();
        if (googleOptions is not null && !string.IsNullOrEmpty(googleOptions.CredentialsPath))
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", googleOptions.CredentialsPath);
        }

        services.AddSingleton(_ => TranslationClient.Create());
        services.AddSingleton<ITranslationProvider, GoogleTranslationProvider>();

        // TRANSLATE SERVICE
        services.AddScoped<ITranslateService, TranslateService>();

        return services;
    }
}
