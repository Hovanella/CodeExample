using Microsoft.EntityFrameworkCore;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Builders;

internal interface IEquipmentQueryBuilder
{
    IEquipmentQueryBuilder WhereAssignOnDate(DateTime? assignDate);

    IEquipmentQueryBuilder WhereAssignWithDepartment(string? departmentId);

    IQueryable<DatabaseEntities.Equipment> Build();
}

internal class EquipmentQueryBuilder(IEquipmentDbContext dbContext) : IEquipmentQueryBuilder
{
    private DateTime? _assignDate;
    private string? _departmentId;

    public IEquipmentQueryBuilder WhereAssignOnDate(DateTime? assignDate)
    {
        _assignDate = assignDate;
        return this;
    }

    public IEquipmentQueryBuilder WhereAssignWithDepartment(string? departmentId)
    {
        _departmentId = departmentId;
        return this;
    }

    public IQueryable<DatabaseEntities.Equipment> Build()
    {
        var query = dbContext.Equipments
            .AsNoTracking()
            .TagWith("Equipment query builder query")
            .Include(equipment => equipment.Type)
            .Include(equipment => equipment.Approver)
            .Include(equipment => equipment.Assigns.Where(a => !a.IsDeleted))
            .ThenInclude(assign => assign.User)
            .AsQueryable();

        if (_assignDate.HasValue)
        {
            query = query.Where(equipment =>
                equipment.Assigns.Any(assign => !assign.IsDeleted &&
                    (assign.ReturnDate == null || assign.ReturnDate.Value.Date >= _assignDate.Value.Date) &&
                    assign.IssueDate.Date <= _assignDate.Value.Date));
        }

        if (_departmentId != null)
        {
            query = query
                .Where(equipment => equipment.Assigns
                    .Where(a => !a.IsDeleted &&
                        (a.ReturnDate == null || a.ReturnDate >= DateTime.UtcNow) &&
                        a.IssueDate <= DateTime.UtcNow)
                    .OrderBy(x => x.IssueDate)
                    .Take(1)
                    .Any(x => x.User!.DepartmentId == _departmentId));
        }

        return query;
    }
}
