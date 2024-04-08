namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments.GetPossibleEquipmentActiveHolder;

internal record GetPossibleEquipmentActiveHoldersDbQueryRequest(
    string? DepartmentId,
    bool IsAvailableEquipmentsWithoutAssigners);
