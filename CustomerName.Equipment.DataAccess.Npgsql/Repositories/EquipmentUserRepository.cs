using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.Repositories;
using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Repositories;

internal class EquipmentUserRepository : IEquipmentUserRepository
{
    private readonly IEquipmentDbContext _equipmentDbContext;

    public EquipmentUserRepository(
        IEquipmentDbContext equipmentDbContext)
    {
        _equipmentDbContext = equipmentDbContext;
    }

    public async Task CreateOrUpdateUserAsync(
        EquipmentUser equipmentUser,
        CancellationToken cancellationToken)
    {
        var user = await _equipmentDbContext.EquipmentUsers
            .FirstOrDefaultAsync(x => x.UserId == equipmentUser.UserId, cancellationToken);
        if (user == null)
        {
            user = new DatabaseEntities.EquipmentUser
            {
                UserId = equipmentUser.UserId,
                IsActive = true,
            };

            _equipmentDbContext.EquipmentUsers.Add(user);
        }

        user.FirstName = equipmentUser.FirstName;
        user.LastName = equipmentUser.LastName;
        user.Email = equipmentUser.Email;
        user.DepartmentId = equipmentUser.DepartmentId;

        await _equipmentDbContext.SaveChangesAsync(cancellationToken);
    }
}
