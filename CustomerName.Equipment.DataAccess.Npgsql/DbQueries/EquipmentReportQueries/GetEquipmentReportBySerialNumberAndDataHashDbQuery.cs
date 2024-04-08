using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.Mappers.EquipmentReports;
using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries.EquipmentReportQueries;

internal class GetEquipmentReportBySerialNumberAndDataHashDbQuery :
    IGetEquipmentReportBySerialNumberAndDataHashDbQuery
{
    private readonly IEquipmentDbContext _equipmentDbContext;
    private readonly IDbEquipmentReportToDomainEquipmentReportMapper _mapper;

    public GetEquipmentReportBySerialNumberAndDataHashDbQuery(
        IEquipmentDbContext equipmentDbContext,
        IDbEquipmentReportToDomainEquipmentReportMapper mapper)
    {
        _equipmentDbContext = equipmentDbContext;
        _mapper = mapper;
    }

    public async Task<EquipmentReport?> GetEquipmentReportBySerialNumberAndDataHashAsync(
        string serialNumber,
        string dataHash,
        CancellationToken cancellationToken)
    {
        var equipmentReport = await _equipmentDbContext.EquipmentReports
            .AsNoTracking()
            .Where(equipmentReport => EF.Functions.ILike(equipmentReport.SerialNumber, serialNumber) &&
                   equipmentReport.DataHash == dataHash)
            .FirstOrDefaultAsync(cancellationToken);

        return _mapper.Map(equipmentReport);
    }
}
