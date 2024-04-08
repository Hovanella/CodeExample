using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Framework.UseCases.Abstractions.Queries;

namespace CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentReportById;

internal record GetEquipmentReportByIdQuery(int Id) : IQuery<EquipmentReportDto>, IEquipmentRequest;
