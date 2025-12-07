using App.Domain.Entities.FlashcardEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Persistence.Configurations.FlashcardConfig;

public class FlashcardConfiguration : IEntityTypeConfiguration<Flashcard>
{
    public void Configure(EntityTypeBuilder<Flashcard> builder)
    {
        // RELATIONS
        builder.HasMany(x => x.FlashcardCategories)
            .WithOne(y => y.Flashcard)
            .HasForeignKey(y => y.FlashcardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.FlashcardOldSessions)
            .WithOne(y => y.Flashcard)
            .HasForeignKey(y => y.FlashcardId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}