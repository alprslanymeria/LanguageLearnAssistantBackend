using App.Domain.Entities.FlashcardEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Persistence.Configurations.FlashcardConfig;

public class FlashcardOldSessionConfiguration : BaseOldSessionConfiguration<FlashcardOldSession>
{
    public override void Configure(EntityTypeBuilder<FlashcardOldSession> builder)
    {
        base.Configure(builder);

        // ENTITY SPECIFIC AYARLAR BURADA YAPILABİLİR.

        // RELATIONS
        builder.HasMany(x => x.FlashcardSessionRows)
            .WithOne(y => y.FlashcardOldSession)
            .HasForeignKey(y => y.FlashcardOldSessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}