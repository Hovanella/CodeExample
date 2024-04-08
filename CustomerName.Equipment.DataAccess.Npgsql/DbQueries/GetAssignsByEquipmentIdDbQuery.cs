using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetAssignsByEquipmentId;
using EquipmentAssign = CustomerName.Portal.Equipment.Domain.EquipmentAssign;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries;

internal class GetAssignsByEquipmentIdDbQuery : IGetAssignsByEquipmentIdDbQuery
{
    private readonly IEquipmentDbContext _context;

    public GetAssignsByEquipmentIdDbQuery(IEquipmentDbContext context)
    {
        _context = context;
    }

    public Task<List<EquipmentAssign>> ExecuteAsync(int equipmentId, CancellationToken cancellationToken)
    {
        return _context.EquipmentAssigns
            .Where(a => !a.IsDeleted && a.EquipmentId == equipmentId)
            .Select(a => new EquipmentAssign
            {
                Id = a.Id,
                IssueDate = a.IssueDate,
                ReturnDate = a.ReturnDate,
            })
            .ToListAsync(cancellationToken);
    }
}
