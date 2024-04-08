namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.CreateEquipmentReport;

public record CreateEquipmentReportDbQueryRequest(string SerialNumber, string Data,
    string DataHash, DateTime AssembledAtUtc);
