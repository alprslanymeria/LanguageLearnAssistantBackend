using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Persistence.Configurations;

public abstract class BaseOldSessionConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        // BU DEGERI SHADOW PROPERTY OLARAK EKLEDIK. AMACIMIZ DOMAIN'I DAHA SADE TUTMAK. ÇÜNKÜ BUNLAR METADATA IÇERIR.
        builder.Property<DateTime>("CreatedAt")
            .HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnAdd();

        // BU BASE ENTITY CONFIG IÇERISINDE ENTITY SPESIFIK DEGERLERI CONFIG ETMEMELIYIZ.
    }
}
