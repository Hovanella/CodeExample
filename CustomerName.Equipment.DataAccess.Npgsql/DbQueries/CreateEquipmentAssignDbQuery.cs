using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.CreateEquipmentAssign;
using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries;

internal class CreateEquipmentAssignDbQuery : ICreateEquipmentAssignDbQuery
{
    private readonly IEquipmentDbContext _context;

    public CreateEquipmentAssignDbQuery(IEquipmentDbContext context)
    {
        _context = context;
    }

    public async Task<CreateEquipmentAssignResponse> ExecuteAsync(EquipmentAssign request, CancellationToken cancellationToken)
    {
        var entity = new DatabaseEntities.EquipmentAssign
        {
            IssueDate = request.IssueDate,
            ReturnDate = null,
            EquipmentId = request.EquipmentId,
            AssignedToUserId = request.AssignedToUserId
        };

        await _context.EquipmentAssigns.AddAsync(
            entity,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return new CreateEquipmentAssignResponse(entity.AssignedToUserId,entity.IssueDate);
    }
}
