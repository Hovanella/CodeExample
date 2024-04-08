using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Configurations;

internal class EquipmentAssignConfiguration : IEntityTypeConfiguration<EquipmentAssign>
{
    public void Configure(EntityTypeBuilder<EquipmentAssign> builder)
    {
        builder.ToTable("EquipmentAssigns");

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

        builder.Property(x => x.IssueDate)
            .IsRequired();

        builder.Property(x => x.ReturnDate)
            .IsRequired(false);

        builder.HasOne(x => x.User)
            .WithMany(x => x.EquipmentAssigns)
            .HasForeignKey(x => x.AssignedToUserId);

        builder.HasOne(x => x.Equipment)
            .WithMany(x => x.Assigns)
            .HasForeignKey(x => x.EquipmentId);
    }
}
