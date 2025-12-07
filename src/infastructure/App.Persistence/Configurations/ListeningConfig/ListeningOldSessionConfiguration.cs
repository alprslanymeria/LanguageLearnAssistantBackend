using App.Domain.Entities.ListeningEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Persistence.Configurations.ListeningConfig;

public class ListeningOldSessionConfiguration : BaseOldSessionConfiguration<ListeningOldSession>
{
    public override void Configure(EntityTypeBuilder<ListeningOldSession> builder)
    {
        base.Configure(builder);

        // ENTITY SPECIFIC AYARLAR BURADA YAPILABİLİR.

        // RELATIONS
        builder.HasMany(x => x.ListeningSessionRows)
            .WithOne(y => y.ListeningOldSession)
            .HasForeignKey(y => y.ListeningOldSessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}