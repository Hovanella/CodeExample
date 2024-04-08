using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.ActivateEquipmentUser;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries;

internal class ActivateEquipmentUserDbQuery : IActivateEquipmentUserDbQuery
{
    private readonly EquipmentDbContext _equipmentDbContext;

    public ActivateEquipmentUserDbQuery(EquipmentDbContext equipmentDbContext)
    {
        _equipmentDbContext = equipmentDbContext;
    }

    public async Task ActivateUserAsync(ActivateEquipmentUserRequest request, CancellationToken cancellationToken)
    {
        var deletedUser = await _equipmentDbContext
            .EquipmentUsers
            .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

        if (deletedUser is not null)
        {
            deletedUser.IsActive = true;

            await _equipmentDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
