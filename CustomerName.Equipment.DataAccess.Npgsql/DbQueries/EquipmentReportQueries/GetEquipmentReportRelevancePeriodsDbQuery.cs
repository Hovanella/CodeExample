using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.Mappers.EquipmentReports;
using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries.EquipmentReportQueries;

internal class GetEquipmentReportRelevancePeriodsDbQuery :
    IGetEquipmentReportRelevancePeriodsDbQuery
{
    private readonly IEquipmentDbContext _equipmentDbContext;
    private readonly IDbReportRelevancePeriodToDomainReportRelevancePeriodMapper _mapper;

    public GetEquipmentReportRelevancePeriodsDbQuery(
        IEquipmentDbContext equipmentDbContext,
        IDbReportRelevancePeriodToDomainReportRelevancePeriodMapper mapper)
    {
        _equipmentDbContext = equipmentDbContext;
        _mapper = mapper;
    }

    public async Task<List<EquipmentReportRelevancePeriod>?> GetEquipmentReportRelevancePeriodsBySerialNumberAsync(
        string serialNumber,
        CancellationToken cancellationToken)
    {
        var equipmentReportRelevancePeriods = await _equipmentDbContext.EquipmentReportRelevancePeriods
            .Include(equipmentReportRelevancePeriod => equipmentReportRelevancePeriod.EquipmentReport)
            .OrderByDescending(equipmentReportRelevancePeriod => equipmentReportRelevancePeriod.ToUtc)
            .Where(equipmentReport => EF.Functions.ILike(equipmentReport.EquipmentReport!.SerialNumber, serialNumber))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return _mapper.Map(equipmentReportRelevancePeriods).ToList();
    }
}
