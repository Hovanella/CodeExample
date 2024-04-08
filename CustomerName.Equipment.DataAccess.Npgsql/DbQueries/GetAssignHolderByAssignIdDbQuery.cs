using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;
using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries;

internal class GetAssignHolderByAssignIdDbQuery : IGetAssignHolderByAssignIdDbQuery
{
    private readonly IEquipmentDbContext _context;

    public GetAssignHolderByAssignIdDbQuery(IEquipmentDbContext context)
    {
        _context = context;
    }

    public Task<ActiveHolderDto> ExecuteAsync(int assignId, CancellationToken cancellationToken)
    {
        return _context.EquipmentAssigns
            .Include(x=>x.User)
            .Where(x => !x.IsDeleted && x.Id == assignId)
            .Select(x => new ActiveHolderDto
            {
                Id = x.User!.UserId,
                FirstName = x.User.FirstName!,
                LastName = x.User.LastName!,
                DepartmentId = x.User.DepartmentId,
                IssueDate = x.IssueDate,
                ReturnDate = x.ReturnDate
            })
            .FirstOrDefaultAsync(cancellationToken)!;
    }
}
