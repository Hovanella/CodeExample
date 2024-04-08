using AutoMapper;
using CustomerName.Portal.Equipment.UseCases.Dto;

namespace CustomerName.Portal.Equipment.UseCases.Mappings;

internal class EquipmentDtoProfile : Profile
{
    public EquipmentDtoProfile()
    {
        CreateMap<Domain.Equipment, EquipmentDto>()
            .ForMember(dto => dto.ActiveHolder, options => options.Ignore());
    }
}
