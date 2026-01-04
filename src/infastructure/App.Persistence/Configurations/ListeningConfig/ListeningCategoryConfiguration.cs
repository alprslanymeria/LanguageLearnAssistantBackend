using App.Domain.Entities.ListeningEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Persistence.Configurations.ListeningConfig;

public class ListeningCategoryConfiguration : IEntityTypeConfiguration<ListeningCategory>
{
    public void Configure(EntityTypeBuilder<ListeningCategory> builder)
    {
        // RELATIONS
        builder.HasMany(x => x.ListeningOldSessions)
            .WithOne(y => y.ListeningCategory)
            .HasForeignKey(y => y.ListeningCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.DeckVideos)
            .WithOne(y => y.ListeningCategory)
            .HasForeignKey(y => y.ListeningCategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
