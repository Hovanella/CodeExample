namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.IsSerialNumberExisting;

public record IsSerialNumberExistingDbRequest(string SerialNumber, int? UpdatingEquipmentId);
