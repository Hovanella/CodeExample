using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.CreateEquipmentReport;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.Mappers.EquipmentReports;
using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Framework.UseCases.Abstractions.Services;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries.EquipmentReportQueries;

internal class CreateEquipmentReportDbQuery : ICreateEquipmentReportDbQuery
{
    private readonly IEquipmentDbContext _equipmentDbContext;
    private readonly ICreateEquipmentReportDbQueryRequestToDbEquipmentReportMapper _createReportRequestToEquipmentReportMapper;
    private readonly IDbEquipmentReportToDomainEquipmentReportMapper _dbEquipmentReportToDomainEquipmentReport;
    private readonly IClockService _clockService;

    public CreateEquipmentReportDbQuery(
        IEquipmentDbContext equipmentDbContext,
        ICreateEquipmentReportDbQueryRequestToDbEquipmentReportMapper createReportRequestToEquipmentReportMapper,
        IDbEquipmentReportToDomainEquipmentReportMapper dbEquipmentReportToDomainEquipmentReport,
        IClockService clockService)
    {
        _equipmentDbContext = equipmentDbContext;
        _createReportRequestToEquipmentReportMapper = createReportRequestToEquipmentReportMapper;
        _dbEquipmentReportToDomainEquipmentReport = dbEquipmentReportToDomainEquipmentReport;
        _clockService = clockService;
    }

    public async Task<EquipmentReport> CreateEquipmentReportAsync(
        CreateEquipmentReportDbQueryRequest request,
        CancellationToken cancellationToken)
    {
        var entity = _createReportRequestToEquipmentReportMapper.Map(request);

        var dateTimeUtcNow = _clockService.UtcNow;

        var equipmentReportRelevancePeriod = new DatabaseEntities.EquipmentReportRelevancePeriod
        {
            FromUtc = dateTimeUtcNow,
            ToUtc = dateTimeUtcNow
        };

        entity.EquipmentReportRelevancePeriods.Add(equipmentReportRelevancePeriod);

        await _equipmentDbContext.EquipmentReports.AddAsync(
            entity,
            cancellationToken);

        await _equipmentDbContext.SaveChangesAsync(cancellationToken);

        return _dbEquipmentReportToDomainEquipmentReport.Map(entity);
    }
}
