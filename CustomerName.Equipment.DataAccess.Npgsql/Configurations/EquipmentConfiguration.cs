using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Framework.Utils;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Configurations;

internal class EquipmentConfiguration : IEntityTypeConfiguration<DatabaseEntities.Equipment>
{
    public void Configure(EntityTypeBuilder<DatabaseEntities.Equipment> builder)
    {
        builder.ToTable("Equipments");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();
        builder.Property(x => x.CreatedById)
            .IsRequired();

        builder.Property(x => x.UpdatedAtUtc)
            .IsRequired(false);
        builder.Property(x => x.UpdatedById)
            .IsRequired(false);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.Location)
            .IsRequired()
            .HasConversion(new EnumConverter<EquipmentLocationType>());

        builder.Property(x => x.SerialNumber)
            .IsRequired()
            .HasMaxLength(250);
        builder.HasIndex(x => x.SerialNumber);

        builder.Property(x => x.PurchasePrice)
            .IsRequired();
        builder.Property(x => x.PurchasePriceUsd)
            .IsRequired();

        builder.Property(x => x.PurchaseCurrency)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.PurchaseDate)
            .IsRequired();

        builder.Property(x => x.PurchasePlace)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.GuaranteeDate)
            .IsRequired();

        builder.Property(x => x.Characteristics)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.Comment)
            .IsRequired(false)
            .HasMaxLength(1000);

        builder.Property(x => x.InvoiceNumber)
            .IsRequired(false)
            .HasMaxLength(250);

        builder.HasOne(x => x.Approver)
            .WithMany(x => x.ApprovedEquipments)
            .HasForeignKey(x => x.ApproverId);

        builder.HasOne(x=>x.Type)
            .WithMany()
            .IsRequired(false)
            .HasForeignKey(x => x.TypeId);

        builder.Property(x => x.TypeId)
            .HasMaxLength(64);

        builder.Property(x => x.TypeId)
            .HasMaxLength(64);
    }
}
