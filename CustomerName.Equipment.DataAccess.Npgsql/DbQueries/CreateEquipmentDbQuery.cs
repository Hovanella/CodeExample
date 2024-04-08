using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries;

internal class CreateEquipmentDbQuery : ICreateEquipmentDbQuery
{
    private readonly IEquipmentDbContext _dbContext;

    public CreateEquipmentDbQuery(IEquipmentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Domain.Equipment> CreateEquipmentAsync(
        Domain.Equipment equipment,
        CancellationToken cancellationToken)
    {
        var entity = new DatabaseEntities.Equipment
        {
            Name = equipment.Name,
            SerialNumber = equipment.SerialNumber,
            PurchasePrice = equipment.PurchasePrice,
            PurchaseCurrency = equipment.PurchaseCurrency,
            PurchasePriceUsd = equipment.PurchasePriceUsd,
            Location = equipment.Location,
            PurchaseDate = equipment.PurchaseDate,
            PurchasePlace = equipment.PurchasePlace,
            GuaranteeDate = equipment.GuaranteeDate,
            Characteristics = equipment.Characteristics,
            Comment = equipment.Comment,
            InvoiceNumber = equipment.InvoiceNumber,
            ApproverId = equipment.ApproverId,
            TypeId = equipment.TypeId
        };

        await _dbContext.Equipments.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        equipment.Id = entity.Id;

        return equipment;
    }
}
