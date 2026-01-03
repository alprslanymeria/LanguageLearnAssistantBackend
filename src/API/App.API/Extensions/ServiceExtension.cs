using App.Application.Features.FlashcardOldSessions;
using App.Application.Features.FlashcardSessionRows;
using App.Application.Features.ListeningOldSessions;
using App.Application.Features.ListeningSessionRows;
using App.Application.Features.ReadingSessionRows;
using App.Application.Features.Translation;
using App.Application.Features.WritingOldSessions;
using App.Application.Features.WritingSessionRows;

namespace App.API.Extensions;

/// <summary>
/// EXTENSION FOR REGISTERING APPLICATION SERVICES WITH DEPENDENCY INJECTION.
/// NOTE: SERVICES THAT USE CQRS PATTERN ARE REGISTERED VIA MEDIATR AUTOMATICALLY.
/// THIS FILE ONLY CONTAINS SERVICES THAT STILL USE THE TRADITIONAL SERVICE PATTERN.
/// </summary>
public static class ServiceExtension
{
    public static IServiceCollection AddApplicationServicesExt(this IServiceCollection services)
    {
        // OLD SESSION SERVICES (REMAINING SERVICES THAT DON'T USE CQRS YET)
        services.AddScoped<IWritingOldSessionService, WritingOldSessionService>();
        services.AddScoped<IFlashcardOldSessionService, FlashcardOldSessionService>();
        services.AddScoped<IListeningOldSessionService, ListeningOldSessionService>();

        // SESSION ROW SERVICES (REMAINING SERVICES THAT DON'T USE CQRS YET)
        services.AddScoped<IReadingSessionRowService, ReadingSessionRowService>();
        services.AddScoped<IWritingSessionRowService, WritingSessionRowService>();
        services.AddScoped<IFlashcardSessionRowService, FlashcardSessionRowService>();
        services.AddScoped<IListeningSessionRowService, ListeningSessionRowService>();

        // TRANSLATION SERVICE
        services.AddScoped<ITranslateService, TranslateService>();

        return services;
    }
}
