using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Framework.UseCases.Abstractions.Queries;

namespace CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentHistory;

internal record GetEquipmentHistoryQuery(int EquipmentId) : IQuery<List<EquipmentHolderDto>>, IEquipmentRequest;
