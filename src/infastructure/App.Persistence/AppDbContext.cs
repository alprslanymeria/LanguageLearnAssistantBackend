using App.Domain.Entities;
using App.Domain.Entities.FlashcardEntities;
using App.Domain.Entities.ListeningEntities;
using App.Domain.Entities.ReadingEntities;
using App.Domain.Entities.WritingEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Language> Languages { get; set; }
    public DbSet<Practice> Practices { get; set; }

    // WRITING
    public DbSet<Writing> Writings { get; set; }
    public DbSet<WritingBook> WritingBooks { get; set; }
    public DbSet<WritingOldSession> WritingOldSessions { get; set; }
    public DbSet<WritingSessionRow> WritingSessionRows { get; set; }

    // READING
    public DbSet<Reading> Readings { get; set; }
    public DbSet<ReadingBook> ReadingBooks { get; set; }
    public DbSet<ReadingOldSession> ReadingOldSessions { get; set; }
    public DbSet<ReadingSessionRow> ReadingSessionRows { get; set; }

    // FLASHCARD
    public DbSet<Flashcard> Flashcards { get; set; }
    public DbSet<FlashcardCategory> FlashcardCategories { get; set; }
    public DbSet<FlashcardOldSession> FlashcardOldSessions { get; set; }
    public DbSet<FlashcardSessionRow> FlashcardSessionRows { get; set; }
    public DbSet<DeckWord> DeckWords { get; set; }

    // LISTENING
    public DbSet<Listening> Listenings { get; set; }
    public DbSet<ListeningCategory> ListeningCategories { get; set; }
    public DbSet<ListeningOldSession> ListeningOldSessions { get; set; }
    public DbSet<ListeningSessionRow> ListeningSessionRows { get; set; }
    public DbSet<DeckVideo> DeckVideos { get; set; }


    // ON MODEL CREATING
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // APPLY CONFIGURATIONS
        // BU ASSEMBLY İÇERİSİNDE "IEntityTypeConfiguration" INTERFACE'İNİ IMPLEMENT EDEN TÜM CLASS'LARI OTOMATİK OLARAK BULUP UYGULAR
        builder.ApplyConfigurationsFromAssembly(typeof(PersistenceAssembly).Assembly);

        base.OnModelCreating(builder);
    }
}
