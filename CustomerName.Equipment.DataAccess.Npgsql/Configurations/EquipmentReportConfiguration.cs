using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Configurations;

internal class EquipmentReportConfiguration : IEntityTypeConfiguration<EquipmentReport>
{
    public void Configure(EntityTypeBuilder<EquipmentReport> builder)
    {
        builder.ToTable("EquipmentReports");

        builder.HasKey(equipmentReport => equipmentReport.Id);

        builder.Property(equipmentReport => equipmentReport.AssembledAtUtc)
            .IsRequired();

        builder.Property(equipmentReport => equipmentReport.SerialNumber)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(equipmentReport => equipmentReport.Data)
            .IsRequired()
            .HasMaxLength(1048576)
            .HasColumnType("json");

        builder.Property(equipmentReport => equipmentReport.DataHash)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(equipmentReport => equipmentReport.CreatedAtUtc)
            .IsRequired()
            .HasDefaultValueSql("now()");
    }
}
