using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.Providers;
using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Providers;

internal class EquipmentUserProvider : IEquipmentUserProvider
{
    private readonly IEquipmentDbContext _context;
    private readonly IMapper _mapper;

    public EquipmentUserProvider(
        IEquipmentDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<EquipmentUser?> GetUserByIdAsync(
        int userId,
        CancellationToken cancellationToken)
    {
        var userEntity = await _context.EquipmentUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.UserId == userId, cancellationToken);

        return _mapper.Map<EquipmentUser?>(userEntity);
    }
}
