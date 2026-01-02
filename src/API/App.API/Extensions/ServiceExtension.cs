using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Features.DeckWords;
using App.Application.Features.FlashcardCategories;
using App.Application.Features.FlashcardOldSessions;
using App.Application.Features.FlashcardSessionRows;
using App.Application.Features.Languages;
using App.Application.Features.ListeningOldSessions;
using App.Application.Features.ListeningSessionRows;
using App.Application.Features.Practices;
using App.Application.Features.ReadingBooks;
using App.Application.Features.ReadingOldSessions;
using App.Application.Features.ReadingSessionRows;
using App.Application.Features.WritingBooks;
using App.Application.Features.WritingOldSessions;
using App.Application.Features.WritingSessionRows;

namespace App.API.Extensions;

/// <summary>
/// EXTENSION FOR REGISTERING APPLICATION SERVICES WITH DEPENDENCY INJECTION.
/// </summary>
public static class ServiceExtension
{
    public static IServiceCollection AddApplicationServicesExt(this IServiceCollection services)
    {

        // LANGUAGE SERVICE WITH CACHE DECORATOR
        services.AddScoped<LanguageService>();
        services.AddScoped<ILanguageService>(provider =>
        {
            var innerService = provider.GetRequiredService<LanguageService>();
            var cacheManager = provider.GetRequiredService<IStaticCacheManager>();
            var cacheKeyFactory = provider.GetRequiredService<ICacheKeyFactory>();
            return new LanguageServiceCacheDecorator(innerService, cacheManager, cacheKeyFactory);
        });

        // PRACTICE SERVICE WITH CACHE DECORATOR
        services.AddScoped<PracticeService>();
        services.AddScoped<IPracticeService>(provider =>
        {
            var innerService = provider.GetRequiredService<PracticeService>();
            var cacheManager = provider.GetRequiredService<IStaticCacheManager>();
            var cacheKeyFactory = provider.GetRequiredService<ICacheKeyFactory>();
            return new PracticeServiceCacheDecorator(innerService, cacheManager, cacheKeyFactory);
        });

        // READING BOOK SERVICE WITH CACHE DECORATOR
        services.AddScoped<ReadingBookService>();
        services.AddScoped<IReadingBookService>(provider =>
        {
            var innerService = provider.GetRequiredService<ReadingBookService>();
            var cacheManager = provider.GetRequiredService<IStaticCacheManager>();
            var cacheKeyFactory = provider.GetRequiredService<ICacheKeyFactory>();
            return new ReadingBookServiceCacheDecorator(innerService, cacheManager, cacheKeyFactory);
        });

        // WRITING BOOK SERVICE WITH CACHE DECORATOR
        services.AddScoped<WritingBookService>();
        services.AddScoped<IWritingBookService>(provider =>
        {
            var innerService = provider.GetRequiredService<WritingBookService>();
            var cacheManager = provider.GetRequiredService<IStaticCacheManager>();
            var cacheKeyFactory = provider.GetRequiredService<ICacheKeyFactory>();
            return new WritingBookServiceCacheDecorator(innerService, cacheManager, cacheKeyFactory);
        });

        // FLASHCARD CATEGORY SERVICE WITH CACHE DECORATOR
        services.AddScoped<FlashcardCategoryService>();
        services.AddScoped<IFlashcardCategoryService>(provider =>
        {
            var innerService = provider.GetRequiredService<FlashcardCategoryService>();
            var cacheManager = provider.GetRequiredService<IStaticCacheManager>();
            var cacheKeyFactory = provider.GetRequiredService<ICacheKeyFactory>();
            return new FlashcardCategoryServiceCacheDecorator(innerService, cacheManager, cacheKeyFactory);
        });

        // DECK WORD SERVICE WITH CACHE DECORATOR
        services.AddScoped<DeckWordService>();
        services.AddScoped<IDeckWordService>(provider =>
        {
            var innerService = provider.GetRequiredService<DeckWordService>();
            var cacheManager = provider.GetRequiredService<IStaticCacheManager>();
            var cacheKeyFactory = provider.GetRequiredService<ICacheKeyFactory>();
            return new DeckWordServiceCacheDecorator(innerService, cacheManager, cacheKeyFactory);
        });

        // OLD SESSION SERVICES (NO CACHING - SESSION DATA IS TRANSIENT)
        services.AddScoped<IReadingOldSessionService, ReadingOldSessionService>();
        services.AddScoped<IWritingOldSessionService, WritingOldSessionService>();
        services.AddScoped<IFlashcardOldSessionService, FlashcardOldSessionService>();
        services.AddScoped<IListeningOldSessionService, ListeningOldSessionService>();

        // SESSION ROW SERVICES (NO CACHING - SESSION DATA IS TRANSIENT)
        services.AddScoped<IReadingSessionRowService, ReadingSessionRowService>();
        services.AddScoped<IWritingSessionRowService, WritingSessionRowService>();
        services.AddScoped<IFlashcardSessionRowService, FlashcardSessionRowService>();
        services.AddScoped<IListeningSessionRowService, ListeningSessionRowService>();

        return services;
    }
}
