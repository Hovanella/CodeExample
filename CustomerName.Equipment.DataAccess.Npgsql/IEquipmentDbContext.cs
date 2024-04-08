using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities;
using CustomerName.Portal.Equipment.Domain.OData;

[assembly: InternalsVisibleTo("CustomerName.Portal.IntegrationTests")]
namespace CustomerName.Portal.Equipment.DataAccess.Npgsql;

internal interface IEquipmentDbContext
{
    public DbSet<DatabaseEntities.Equipment> Equipments { get; }
    public DbSet<EquipmentAssign> EquipmentAssigns { get; }
    public DbSet<EquipmentUser> EquipmentUsers { get; }
    public DbSet<EquipmentType> EquipmentTypes { get; }
    public DbSet<OdpEquipment> OdataEquipments { get; }
    public DbSet<EquipmentReport> EquipmentReports { get; }
    public DbSet<EquipmentReportRelevancePeriod> EquipmentReportRelevancePeriods { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
