using AutoMapper;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Mappings;

internal class EquipmentUserProfile : Profile
{
    public EquipmentUserProfile()
    {
        CreateMap<EquipmentUser, Domain.EquipmentUser>().ReverseMap();
    }
}
