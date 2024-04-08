using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities;
using CustomerName.Portal.Framework.UseCases.Abstractions.Services;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries.EquipmentReportQueries;

internal class UpdateEquipmentReportRelevancePeriodDbQuery :
    IUpdateEquipmentReportRelevancePeriodDbQuery
{
    private readonly IEquipmentDbContext _equipmentDbContext;
    private readonly IClockService _clockService;

    public UpdateEquipmentReportRelevancePeriodDbQuery(
        IEquipmentDbContext equipmentDbContext,
        IClockService clockService)
    {
        _equipmentDbContext = equipmentDbContext;
        _clockService = clockService;
    }

    public async Task UpdateEquipmentReportRelevancePeriodToUtcDateAsync(
        string serialNumber,
        int equipmentReportId,
        CancellationToken cancellationToken)
    {
        var relevantReportPeriod = await _equipmentDbContext.EquipmentReportRelevancePeriods
            .Include(reportRelevancePeriod =>
                     reportRelevancePeriod.EquipmentReport)
            .OrderByDescending(reportRelevancePeriod => reportRelevancePeriod.ToUtc)
            .Where(reportRelevancePeriod =>
                   reportRelevancePeriod.EquipmentReport!.SerialNumber == serialNumber)
            .FirstOrDefaultAsync(cancellationToken);

        if (relevantReportPeriod?.EquipmentReportId == equipmentReportId)
        {
            relevantReportPeriod.ToUtc = _clockService.UtcNow;
        }
        else if (relevantReportPeriod is not null)
        {
            var nowDateTimeUtc = _clockService.UtcNow;

            var period = new EquipmentReportRelevancePeriod
            {
                EquipmentReportId = equipmentReportId,
                ToUtc = nowDateTimeUtc,
                FromUtc = nowDateTimeUtc
            };

            await _equipmentDbContext.EquipmentReportRelevancePeriods.AddAsync(period);
        }

        await _equipmentDbContext.SaveChangesAsync(cancellationToken);
    }
}
