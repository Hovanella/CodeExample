using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.UpdateEquipmentDbQuery;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries;

internal class UpdateEquipmentDbQuery : IUpdateEquipmentDbQuery
{
    private readonly IEquipmentDbContext _context;

    public UpdateEquipmentDbQuery(IEquipmentDbContext context)
    {
        _context = context;
    }

    public async Task<Task> ExecuteAsync(UpdateEquipmentDbQueryRequest request, CancellationToken cancellationToken)
    {
        var equipment = await _context.Equipments
            .Include(x => x.Type)
            .Include(x => x.Approver)
            .FirstAsync(x => x.Id == request.Id, cancellationToken);

        var equipmentType = await _context.EquipmentTypes
            .FirstAsync(x => x.Id == request.TypeId, cancellationToken);

        var equipmentApprover = await _context.EquipmentUsers
            .FirstAsync(x => x.UserId == request.ApproverId, cancellationToken);

        equipment.Name = request.Name;
        equipment.Type = equipmentType;
        equipment.Location = request.Location;
        equipment.SerialNumber = request.SerialNumber;
        equipment.PurchasePrice = request.PurchasePrice;
        equipment.PurchaseCurrency = request.PurchaseCurrency;
        equipment.PurchasePriceUsd = request.PurchasePriceUsd;
        equipment.PurchaseDate = request.PurchaseDate;
        equipment.PurchasePlace = request.PurchasePlace;
        equipment.GuaranteeDate = request.GuaranteeDate;
        equipment.Characteristics = request.Characteristics;
        equipment.Comment = request.Comment;
        equipment.InvoiceNumber = request.InvoiceNumber;
        equipment.Approver = equipmentApprover;

        await _context.SaveChangesAsync(cancellationToken);

        return Task.CompletedTask;
    }
}
