using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Configurations;

public class EquipmentTypeConfiguration : IEntityTypeConfiguration<EquipmentType>
{
    public void Configure(EntityTypeBuilder<EquipmentType> builder)
    {
        builder.ToTable("EquipmentTypes", EquipmentDbContext.Schema);

        builder.Property(x => x.CreatedAtUtc).IsRequired();
        builder.Property(x => x.CreatedById).IsRequired();

        builder.Property(x => x.UpdatedAtUtc).IsRequired(false);
        builder.Property(x => x.UpdatedById).IsRequired(false);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(x => x.ShortName)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(128);
    }
}
