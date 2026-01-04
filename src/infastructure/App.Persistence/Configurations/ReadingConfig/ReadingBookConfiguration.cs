using App.Domain.Entities.ReadingEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Persistence.Configurations.ReadingConfig;

public class ReadingBookConfiguration : IEntityTypeConfiguration<ReadingBook>
{
    public void Configure(EntityTypeBuilder<ReadingBook> builder)
    {
        builder.HasMany(x => x.ReadingOldSessions)
            .WithOne(y => y.ReadingBook)
            .HasForeignKey(y => y.ReadingBookId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
