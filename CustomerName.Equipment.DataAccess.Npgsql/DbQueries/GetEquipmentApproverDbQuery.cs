using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;
using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries;

internal class GetEquipmentApproverDbQuery : IGetEquipmentApproverDbQuery
{
    private readonly IEquipmentDbContext _context;

    public GetEquipmentApproverDbQuery(IEquipmentDbContext context)
    {
        _context = context;
    }

    public Task<EquipmentUser?> ExecuteAsync(int approverId, CancellationToken cancellationToken)
    {
        return _context.EquipmentUsers
            .Where(x => x.UserId == approverId && x.IsActive)
            .Select(x => new EquipmentUser
            {
                UserId = x.UserId,
                FirstName = x.FirstName!,
                LastName = x.LastName!,
                Email = x.Email!,
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
