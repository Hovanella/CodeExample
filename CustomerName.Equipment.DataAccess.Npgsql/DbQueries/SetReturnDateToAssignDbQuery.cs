using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.SetReturnDateToAssign;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries;

internal class SetReturnDateToAssignDbQuery : ISetReturnDateToAssignDbQuery
{
    private readonly IEquipmentDbContext _context;

    public SetReturnDateToAssignDbQuery(IEquipmentDbContext context)
    {
        _context = context;
    }

    public async Task ExecuteAsync(SetReturnDateToAssignRequest request, CancellationToken cancellationToken)
    {
        var assign = await _context
            .EquipmentAssigns
            .FirstAsync(assign => !assign.IsDeleted && assign.Id == request.AssignId, cancellationToken);

        assign.ReturnDate = request.ReturnDate;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
