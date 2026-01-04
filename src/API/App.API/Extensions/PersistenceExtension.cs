using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Persistence;
using App.Persistence.Interceptors;
using App.Persistence.Repositories;

namespace App.API.Extensions;

public static class PersistenceExtension
{
    public static IServiceCollection AddPersistenceServicesExt(this IServiceCollection services, IConfiguration configuration)
    {
        // CONNECTION STRING
        var connString = configuration.GetConnectionString("SqlServer");

        // HTTP CONTEXT ACCESSOR (REQUIRED FOR AUDIT INTERCEPTOR)
        services.AddHttpContextAccessor();

        // AUDIT INTERCEPTOR
        services.AddScoped<AuditSaveChangesInterceptor>();

        // DB CONTEXT WITH INTERCEPTOR
        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            var auditInterceptor = sp.GetRequiredService<AuditSaveChangesInterceptor>();
            DbContextConfigurator.Configure(options, connString!, typeof(PersistenceAssembly).Assembly.FullName!);
            options.AddInterceptors(auditInterceptor);
        });

        // UNIT OF WORK
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // ENTITY-SPECIFIC REPOSITORIES
        services.AddScoped<ILanguageRepository, LanguageRepository>();
        services.AddScoped<IPracticeRepository, PracticeRepository>();
        services.AddScoped<IReadingRepository, ReadingRepository>();
        services.AddScoped<IReadingBookRepository, ReadingBookRepository>();
        services.AddScoped<IReadingOldSessionRepository, ReadingOldSessionRepository>();
        services.AddScoped<IReadingSessionRowRepository, ReadingSessionRowRepository>();
        services.AddScoped<IWritingRepository, WritingRepository>();
        services.AddScoped<IWritingBookRepository, WritingBookRepository>();
        services.AddScoped<IWritingOldSessionRepository, WritingOldSessionRepository>();
        services.AddScoped<IWritingSessionRowRepository, WritingSessionRowRepository>();
        services.AddScoped<IFlashcardRepository, FlashcardRepository>();
        services.AddScoped<IFlashcardCategoryRepository, FlashcardCategoryRepository>();
        services.AddScoped<IFlashcardOldSessionRepository, FlashcardOldSessionRepository>();
        services.AddScoped<IFlashcardSessionRowRepository, FlashcardSessionRowRepository>();
        services.AddScoped<IDeckWordRepository, DeckWordRepository>();
        services.AddScoped<IListeningRepository, ListeningRepository>();
        services.AddScoped<IListeningCategoryRepository, ListeningCategoryRepository>();
        services.AddScoped<IListeningOldSessionRepository, ListeningOldSessionRepository>();
        services.AddScoped<IListeningSessionRowRepository, ListeningSessionRowRepository>();

        return services;
    }
}
