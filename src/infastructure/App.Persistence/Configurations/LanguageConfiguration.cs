using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Persistence.Configurations;

// FLASHCARD - LISTENING - WRITING - READING BUNLAR USER SILINDIGINDE BURADAN SILINMELI
public class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        // RELATIONS

        builder.HasMany(x => x.Practices)
            .WithOne(y => y.Language)
            .HasForeignKey(y => y.LanguageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Flashcards)
            .WithOne(y => y.Language)
            .HasForeignKey(y => y.LanguageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Listenings)
            .WithOne(y => y.Language)
            .HasForeignKey(y => y.LanguageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Writings)
            .WithOne(y => y.Language)
            .HasForeignKey(y => y.LanguageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Readings)
            .WithOne(y => y.Language)
            .HasForeignKey(y => y.LanguageId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
