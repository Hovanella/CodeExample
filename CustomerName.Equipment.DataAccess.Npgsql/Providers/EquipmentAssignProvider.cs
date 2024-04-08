using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.Providers;
using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Providers;

internal class EquipmentAssignProvider : IEquipmentAssignProvider
{
    private readonly IEquipmentDbContext _context;
    private readonly IMapper _mapper;

    public EquipmentAssignProvider(
        IEquipmentDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<List<EquipmentAssign>> GetByEquipmentIdAsync(
        int equipmentId,
        CancellationToken cancellationToken)
    {
        return _context.EquipmentAssigns
            .AsNoTracking()
            .Include(assign => assign.User)
            .Where(assign => !assign.IsDeleted && assign.EquipmentId == equipmentId)
            .OrderByDescending(assign => !assign.ReturnDate.HasValue)
            .ThenByDescending(assign => assign.IssueDate)
            .ProjectTo<EquipmentAssign>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public Task<List<EquipmentAssign>> GetEquipmentsByUserIdAsync(
        int userId,
        CancellationToken cancellationToken)
    {
        return _context.EquipmentAssigns
            .AsNoTracking()
            .Include(assign => assign.Equipment)
            .Where(assign => !assign.IsDeleted && assign.AssignedToUserId == userId)
            .OrderByDescending(assign => !assign.ReturnDate.HasValue)
            .ThenByDescending(assign => assign.IssueDate)
            .ProjectTo<EquipmentAssign>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
