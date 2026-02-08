using App.Domain.Options.ExternalAPI;

namespace App.API.Extensions;

public static class OptionsExtension
{
    public static IServiceCollection AddOptionsPatternExt(this IServiceCollection services, IConfiguration configuration)
    {
        // OPTIONS PATTERN
        services.Configure<UserApiOptions>(configuration.GetSection(UserApiOptions.Key));

        return services;
    }
}
