using AutoMapper;
using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Equipment.UseCases.Dto;

namespace CustomerName.Portal.Equipment.UseCases.Mappings;

public class EquipmentTypeDtoProfile : Profile
{
    public EquipmentTypeDtoProfile()
    {
        CreateMap<EquipmentType, EquipmentTypeDto>();
    }
}
