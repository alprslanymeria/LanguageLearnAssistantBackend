using App.Domain.Entities.ReadingEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Persistence.Configurations.ReadingConfig;

public class ReadingConfiguration : IEntityTypeConfiguration<Reading>
{
    public void Configure(EntityTypeBuilder<Reading> builder)
    {
        builder.HasMany(x => x.ReadingBooks)
            .WithOne(y => y.Reading)
            .HasForeignKey(y => y.ReadingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.ReadingOldSessions)
            .WithOne(y => y.Reading)
            .HasForeignKey(y => y.ReadingId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
