using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.IsSerialNumberExisting;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries;

internal class IsSerialNumberExistingDbQuery : IIsSerialNumberExistingDbQuery
{
    private readonly IEquipmentDbContext _dbContext;

    public IsSerialNumberExistingDbQuery(IEquipmentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> ExecuteAsync(IsSerialNumberExistingDbRequest request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Equipments.Where(equipment => equipment.SerialNumber == request.SerialNumber);

        if (request.UpdatingEquipmentId is not null)
        {
            query = query.Where(equipment => !(equipment.SerialNumber == request.SerialNumber &&
                                               equipment.Id == request.UpdatingEquipmentId));
        }

        return query.AnyAsync(cancellationToken);
    }
}
