using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities;
using CustomerName.Portal.Equipment.Domain.OData;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql;

internal class EquipmentDbContext : DbContext, IEquipmentDbContext
{
    public static readonly string Schema = "equipment";

    public DbSet<DatabaseEntities.Equipment> Equipments { get; set; }
    public DbSet<EquipmentAssign> EquipmentAssigns { get; set; }
    public DbSet<EquipmentUser> EquipmentUsers { get; set; }
    public DbSet<EquipmentType> EquipmentTypes { get; set; }
    public DbSet<OdpEquipment> OdataEquipments { get; set; }
    public DbSet<EquipmentReport> EquipmentReports { get; set; }
    public DbSet<EquipmentReportRelevancePeriod> EquipmentReportRelevancePeriods { get; set; }

    public EquipmentDbContext(DbContextOptions<EquipmentDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema(Schema)
            .ApplyConfigurationsFromAssembly(typeof(EquipmentDbContext).Assembly);
    }
}
