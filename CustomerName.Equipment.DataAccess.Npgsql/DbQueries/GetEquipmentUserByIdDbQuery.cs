using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;
using EquipmentUser = CustomerName.Portal.Equipment.Domain.EquipmentUser;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries;

internal class GetEquipmentUserByIdDbQuery : IGetEquipmentUserByIdDbQuery
{
    private readonly IEquipmentDbContext _context;

    public GetEquipmentUserByIdDbQuery(IEquipmentDbContext context)
    {
        _context = context;
    }

    public Task<EquipmentUser?> ExecuteAsync(int userId, CancellationToken cancellationToken)
    {
        return _context.EquipmentUsers
            .Where(x => x.IsActive)
            .Select(x => new EquipmentUser
            {
                UserId = x.UserId,
                FirstName = x.FirstName!,
                LastName = x.LastName!,
                Email = x.Email!,
                DepartmentId = x.DepartmentId
            })
            .FirstOrDefaultAsync(entity => entity.UserId == userId, cancellationToken);
    }
}
