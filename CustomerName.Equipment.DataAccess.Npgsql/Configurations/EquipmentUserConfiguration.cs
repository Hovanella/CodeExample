using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities;
using CustomerName.Portal.Framework.Core;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Configurations;

internal class EquipmentUserConfiguration : IEntityTypeConfiguration<EquipmentUser>
{
    public void Configure(EntityTypeBuilder<EquipmentUser> builder)
    {
        builder.ToTable("EquipmentUsers");

        builder.HasKey(x => x.UserId);
        builder.Property(x => x.UserId)
            .ValueGeneratedNever();

        builder.Property(x => x.DepartmentId)
            .IsRequired(false)
            .HasMaxLength(64);

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(LengthConstants.EmailMaxLength);
        builder.HasIndex(x => x.Email);

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(value: true);
        builder.HasIndex(x => x.IsActive);
    }
}
