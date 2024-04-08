using AutoMapper;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Mappings;

internal class EquipmentProfile : Profile
{
    public EquipmentProfile()
    {
        CreateMap<DatabaseEntities.Equipment, Domain.Equipment>().ReverseMap();
    }
}
