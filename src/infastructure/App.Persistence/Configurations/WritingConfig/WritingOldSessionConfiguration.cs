using App.Domain.Entities.WritingEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Persistence.Configurations.WritingConfig;

public class WritingOldSessionConfiguration : BaseOldSessionConfiguration<WritingOldSession>
{
    public override void Configure(EntityTypeBuilder<WritingOldSession> builder)
    {
        base.Configure(builder);

        // ENTITY SPECIFIC AYARLAR BURADA YAPILABİLİR.

        // RELATIONS
        builder.HasMany(x => x.WritingSessionRows)
            .WithOne(y => y.WritingOldSession)
            .HasForeignKey(y => y.WritingOldSessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}