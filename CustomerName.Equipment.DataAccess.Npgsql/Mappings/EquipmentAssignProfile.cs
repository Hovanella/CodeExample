using AutoMapper;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Mappings;

internal class EquipmentAssignProfile : Profile
{
    public EquipmentAssignProfile()
    {
        CreateMap<EquipmentAssign, Domain.EquipmentAssign>().ReverseMap();
    }
}
