using AutoMapper;
using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Equipment.UseCases.Dto;

namespace CustomerName.Portal.Equipment.UseCases.Mappings;

internal class EquipmentAssignProfile : Profile
{
    public EquipmentAssignProfile()
    {
        CreateMap<EquipmentAssign, UserEquipmentDto>()
            .ForMember(dto => dto.EquipmentTypeId, options => options.MapFrom(x => x.Equipment!.TypeId))
            .ForMember(dto => dto.EquipmentName, options => options.MapFrom(x => x.Equipment!.Name))
            .ForMember(dto => dto.SerialNumber, options => options.MapFrom(x => x.Equipment!.SerialNumber))
            .ForMember(dto => dto.IssueDate, options => options.MapFrom(x => x.IssueDate))
            .ForMember(dto => dto.ReturnDate, options => options.MapFrom(x => x.ReturnDate));
    }
}