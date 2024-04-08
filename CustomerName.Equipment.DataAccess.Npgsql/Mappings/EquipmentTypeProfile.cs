using AutoMapper;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Mappings;

public class EquipmentTypeProfile : Profile
{
    public EquipmentTypeProfile()
    {
        CreateMap<EquipmentType, Domain.EquipmentType>().ReverseMap();
    }
}
