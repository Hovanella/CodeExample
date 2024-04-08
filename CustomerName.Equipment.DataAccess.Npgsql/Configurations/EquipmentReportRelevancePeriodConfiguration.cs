using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Configurations;

internal class EquipmentReportRelevancePeriodConfiguration :
    IEntityTypeConfiguration<EquipmentReportRelevancePeriod>
{
    public void Configure(EntityTypeBuilder<EquipmentReportRelevancePeriod> builder)
    {
        builder.ToTable("EquipmentReportRelevancePeriods");

        builder.HasKey(reportRelevancePeriod => new
        {
            reportRelevancePeriod.EquipmentReportId,
            reportRelevancePeriod.FromUtc
        });

        builder.Property(reportRelevancePeriod => reportRelevancePeriod.ToUtc)
            .IsRequired();

        builder.HasOne(reportRelevancePeriod => reportRelevancePeriod.EquipmentReport)
            .WithMany(equipmentReport => equipmentReport.EquipmentReportRelevancePeriods)
            .HasForeignKey(reportRelevancePeriod => reportRelevancePeriod.EquipmentReportId);
    }
}
