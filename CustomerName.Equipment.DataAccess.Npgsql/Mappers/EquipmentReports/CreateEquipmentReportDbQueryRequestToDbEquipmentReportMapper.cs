using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.CreateEquipmentReport;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Mappers.EquipmentReports;

internal interface ICreateEquipmentReportDbQueryRequestToDbEquipmentReportMapper
{
    EquipmentReport Map(CreateEquipmentReportDbQueryRequest request);
}

internal class CreateEquipmentReportDbQueryRequestToDbEquipmentReportMapper :
    ICreateEquipmentReportDbQueryRequestToDbEquipmentReportMapper
{
    public EquipmentReport Map(CreateEquipmentReportDbQueryRequest request)
    {
        return new EquipmentReport
        {
            SerialNumber = request.SerialNumber,
            Data = request.Data,
            DataHash = request.DataHash,
            AssembledAtUtc = request.AssembledAtUtc
        };
    }
}
