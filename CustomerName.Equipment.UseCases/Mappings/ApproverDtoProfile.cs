using AutoMapper;
using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Equipment.UseCases.Dto;

namespace CustomerName.Portal.Equipment.UseCases.Mappings;

internal class ApproverDtoProfile : Profile
{
    public ApproverDtoProfile()
    {
        CreateMap<EquipmentUser, ApproverDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId));
    }
}
