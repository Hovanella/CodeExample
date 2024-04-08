namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;

internal interface ICreateEquipmentDbQuery
{
    Task<Domain.Equipment> CreateEquipmentAsync(Domain.Equipment equipment, CancellationToken cancellationToken);
}
