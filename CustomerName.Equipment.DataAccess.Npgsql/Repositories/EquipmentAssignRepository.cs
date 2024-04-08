using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.Repositories;
using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Framework.Exceptions;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Repositories;

internal class EquipmentAssignRepository : IEquipmentAssignRepository
{
    private readonly IEquipmentDbContext _context;
    private readonly IMapper _mapper;

    public EquipmentAssignRepository(
        IEquipmentDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<EquipmentAssign> DeleteEquipmentAssignmentAsync(
        int assignId,
        CancellationToken cancellationToken)
    {
        var entity = await _context.EquipmentAssigns.FirstOrDefaultAsync(assign =>
                !assign.IsDeleted &&
                assign.Id == assignId,
            cancellationToken) ?? throw new EntityNotFoundException();

        entity.IsDeleted = true;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<EquipmentAssign>(entity);
    }
}
