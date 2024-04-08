using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CustomerName.Portal.Equipment.Domain.OData;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Configurations;

public class OdpEquipmentConfiguration : IEntityTypeConfiguration<OdpEquipment>
{
    public void Configure(EntityTypeBuilder<OdpEquipment> builder)
    {
        builder
            .ToView("EquipmentOdataView")
            .HasKey(x => x.Id);
    }

}
