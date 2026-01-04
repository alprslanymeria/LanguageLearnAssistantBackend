using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Persistence.Configurations;

// FLASHCARD - LISTENING - WRITING - READING BUNLAR LANGUAGE SILINDIGINDE SILINECEK FAKAT PRACTICE SILINDIGINDE SILINMEYECEK.
// IKISINDEN BIRINI TERCIH ETMELIYDIM.
public class PracticeConfiguration : IEntityTypeConfiguration<Practice>
{
    public void Configure(EntityTypeBuilder<Practice> builder)
    {
        // RELATIONS
        builder.HasMany(x => x.Flashcards)
            .WithOne(y => y.Practice)
            .HasForeignKey(y => y.PracticeId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Listenings)
            .WithOne(y => y.Practice)
            .HasForeignKey(y => y.PracticeId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Writings)
            .WithOne(y => y.Practice)
            .HasForeignKey(y => y.PracticeId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Readings)
            .WithOne(y => y.Practice)
            .HasForeignKey(y => y.PracticeId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
