using App.Application.Features.Languages.Services;
using App.Application.Features.Practices.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace App.Application.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // FLUENT VALIDATION
        services.AddValidatorsFromAssembly(typeof(ApplicationAssembly).Assembly);

        // SERVICES
        services.AddScoped<ILanguageService, LanguageService>();
        services.AddScoped<IPracticeService, PracticeService>();

        return services;
    }
}