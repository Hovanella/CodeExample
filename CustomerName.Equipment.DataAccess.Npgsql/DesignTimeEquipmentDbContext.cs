using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql;

internal class DesignTimeEquipmentDbContext : IDesignTimeDbContextFactory<EquipmentDbContext>
{
    public EquipmentDbContext CreateDbContext(string[] args)
    {
        const string connectionString = "Host=localhost;Port=5432;Database=TrackDb;User Id=postgres;Password=postgres";
        var optionsBuilder = new DbContextOptionsBuilder<EquipmentDbContext>();

        optionsBuilder.UseNpgsql(connectionString);

        return new EquipmentDbContext(optionsBuilder.Options);
    }
}
