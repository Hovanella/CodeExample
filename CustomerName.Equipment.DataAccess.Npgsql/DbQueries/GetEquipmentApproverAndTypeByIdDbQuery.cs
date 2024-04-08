using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries;

internal class GetEquipmentApproverAndTypeByIdDbQuery : IGetEquipmentApproverAndTypeByIdDbQuery
{
    private readonly IEquipmentDbContext _dbContext;

    public GetEquipmentApproverAndTypeByIdDbQuery(IEquipmentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Domain.Equipment?> ExecuteAsync(int request, CancellationToken cancellationToken)
    {
        return _dbContext.Equipments
            .AsNoTracking()
            .Include(equipment => equipment.Approver)
            .Include(equipment => equipment.Type)
            .Select(equipment => new Domain.Equipment()
            {
                Id = equipment.Id,
                Name = string.Empty,
                PurchasePlace = string.Empty,
                Characteristics = string.Empty,
                SerialNumber = string.Empty,
                Type = new Domain.EquipmentType(equipment.Type.Id, equipment.Type.ShortName, equipment.Type.FullName),
                Approver = new Domain.EquipmentUser()
                {
                    UserId = equipment.Approver.UserId,
                    FirstName = equipment.Approver.FirstName,
                    LastName = equipment.Approver.LastName,
                    Email = equipment.Approver.Email,
                    DepartmentId = equipment.Approver.DepartmentId
                }
            })
            .FirstOrDefaultAsync(equipment => equipment.Id == request, cancellationToken);
    }
}
