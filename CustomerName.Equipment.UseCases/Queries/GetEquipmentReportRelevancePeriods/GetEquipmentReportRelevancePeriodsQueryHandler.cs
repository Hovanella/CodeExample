using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;
using CustomerName.Portal.Equipment.UseCases.Constants;
using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Equipment.UseCases.Mappers.EquipmentReports;
using CustomerName.Portal.Framework.Exceptions;
using CustomerName.Portal.Framework.UseCases.Abstractions.Queries;

namespace CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentReportRelevancePeriods;

internal class GetEquipmentReportRelevancePeriodsQueryHandler :
    IQueryHandler<GetEquipmentReportRelevancePeriodsQuery, List<EquipmentReportRelevancePeriodDto>>
{
    private readonly IGetEquipmentReportRelevancePeriodsDbQuery _getEquipmentReportRelevancePeriods;
    private readonly IReportRelevancePeriodToReportRelevancePeriodDtoMapper _mapper;

    public GetEquipmentReportRelevancePeriodsQueryHandler(
        IGetEquipmentReportRelevancePeriodsDbQuery getEquipmentReportRelevancePeriods,
        IReportRelevancePeriodToReportRelevancePeriodDtoMapper mapper)
    {
        _getEquipmentReportRelevancePeriods = getEquipmentReportRelevancePeriods;
        _mapper = mapper;
    }

    public async Task<List<EquipmentReportRelevancePeriodDto>> Handle(
        GetEquipmentReportRelevancePeriodsQuery request,
        CancellationToken cancellationToken)
    {
        var equipmentReportRelevancePeriods = await _getEquipmentReportRelevancePeriods
                                                    .GetEquipmentReportRelevancePeriodsBySerialNumberAsync(
                                                        request.SerialNumber,
                                                        cancellationToken);

        if (equipmentReportRelevancePeriods is null || equipmentReportRelevancePeriods.Count == 0)
        {
            throw new EntityNotFoundException(ExceptionConstants.EquipmentReportRelevancePeriodsNotFound);
        }

        return _mapper.Map(equipmentReportRelevancePeriods).ToList();
    }
}
