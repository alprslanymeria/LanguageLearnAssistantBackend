using App.Application.Features.Languages.Services;
using App.Application.Features.Practices.Services;
using App.Application.Features.Flashcards.Services;
using App.Application.Features.Listenings.Services;
using App.Application.Features.Readings.Services;
using App.Application.Features.Writings.Services;
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
        services.AddScoped<IFlashcardService, FlashcardService>();
        services.AddScoped<IListeningService, ListeningService>();
        services.AddScoped<IReadingService, ReadingService>();
        services.AddScoped<IWritingService, WritingService>();

        return services;
    }
}