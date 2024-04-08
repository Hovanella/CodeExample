using AutoMapper;
using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Equipment.UseCases.Dto;

namespace CustomerName.Portal.Equipment.UseCases.Mappings;

internal class EquipmentHolderDtoProfile : Profile
{
    public EquipmentHolderDtoProfile()
    {
        CreateMap<EquipmentAssign, EquipmentHolderDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AssignedToUserId))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User!.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User!.LastName))
            .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.User!.DepartmentId))
            .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => src.IssueDate))
            .ForMember(dest => dest.ReturnDate, opt => opt.MapFrom(src => src.ReturnDate));
    }
}
