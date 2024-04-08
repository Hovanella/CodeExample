using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Framework.UseCases.Abstractions.Queries;

namespace CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentReportRelevancePeriods;

internal record GetEquipmentReportRelevancePeriodsQuery(string SerialNumber) :
    IQuery<List<EquipmentReportRelevancePeriodDto>>,
    IEquipmentRequest;
