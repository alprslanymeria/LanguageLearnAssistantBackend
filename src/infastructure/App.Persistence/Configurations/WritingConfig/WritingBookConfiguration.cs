using App.Domain.Entities.WritingEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Persistence.Configurations.WritingConfig;

public class WritingBookConfiguration : IEntityTypeConfiguration<WritingBook>
{
    public void Configure(EntityTypeBuilder<WritingBook> builder)
    {
        // RELATIONS
        builder.HasMany(x => x.WritingOldSessions)
            .WithOne(y => y.WritingBook)
            .HasForeignKey(y => y.WritingBookId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}