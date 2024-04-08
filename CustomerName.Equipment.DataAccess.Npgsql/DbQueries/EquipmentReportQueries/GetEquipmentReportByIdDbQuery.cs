using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.Mappers.EquipmentReports;
using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries.EquipmentReportQueries;

internal class GetEquipmentReportByIdDbQuery : IGetEquipmentReportByIdDbQuery
{
    private readonly IEquipmentDbContext _equipmentDbContext;
    private readonly IDbEquipmentReportToDomainEquipmentReportMapper _mapper;

    public GetEquipmentReportByIdDbQuery(
        IEquipmentDbContext equipmentDbContext,
        IDbEquipmentReportToDomainEquipmentReportMapper mapper)
    {
        _equipmentDbContext = equipmentDbContext;
        _mapper = mapper;
    }

    public async Task<EquipmentReport?> GetEquipmentReportByIdAsync(
        int equipmentReportId,
        CancellationToken cancellationToken)
    {
        var equipmentReport = await _equipmentDbContext.EquipmentReports
            .AsNoTracking()
            .FirstOrDefaultAsync(
            equipmentReport => equipmentReport.Id == equipmentReportId, cancellationToken);

        return _mapper.Map(equipmentReport);
    }
}
