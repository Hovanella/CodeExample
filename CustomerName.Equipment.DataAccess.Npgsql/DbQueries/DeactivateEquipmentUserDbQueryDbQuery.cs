using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.DeleteEquipmentUser;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries;

internal class DeactivateEquipmentUserDbQueryDbQuery : IDeactivateEquipmentUserDbQuery
{
    private readonly IEquipmentDbContext _equipmentDbContext;

    public DeactivateEquipmentUserDbQueryDbQuery(IEquipmentDbContext equipmentDbContext)
    {
        _equipmentDbContext = equipmentDbContext;
    }

    public async Task DeactivateUserAsync(DeactivateEquipmentUserDbQueryRequest request, CancellationToken cancellationToken)
    {
        var deletedUser = await _equipmentDbContext
            .EquipmentUsers
            .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

        if (deletedUser is not null)
        {
            deletedUser.IsActive = false;

            await _equipmentDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
