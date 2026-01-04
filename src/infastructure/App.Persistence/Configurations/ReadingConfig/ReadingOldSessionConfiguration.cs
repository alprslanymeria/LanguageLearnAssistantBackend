using App.Domain.Entities.ReadingEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Persistence.Configurations.ReadingConfig;

public class ReadingOldSessionConfiguration : BaseOldSessionConfiguration<ReadingOldSession>
{
    public override void Configure(EntityTypeBuilder<ReadingOldSession> builder)
    {
        base.Configure(builder);

        // ENTITY SPECIFIC AYARLAR BURADA YAPILABILIR.

        // RELATIONS
        builder.HasMany(x => x.ReadingSessionRows)
            .WithOne(y => y.ReadingOldSession)
            .HasForeignKey(y => y.ReadingOldSessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
