using AutoMapper;
using CustomerName.Portal.Equipment.Controllers.Contracts.Output;
using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Equipment.UseCases.Dto.OData;
using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Framework.Utils.Extensions;

namespace CustomerName.Portal.Equipment.Controllers.Mappings;

internal class EquipmentOutputProfile : Profile
{
    public EquipmentOutputProfile()
    {
        CreateMap<UseCases.Dto.ApproverDto, ApproverOutput>()
            .ForMember(x => x.FullName, opt => opt.MapFrom(x => $"{x.LastName} {x.FirstName}"));
        CreateMap<UseCases.Dto.OData.ApproverDto, ApproverOutput>()
            .ForMember(x => x.FullName, opt => opt.MapFrom(x => $"{x.LastName} {x.FirstName}"));
        CreateMap<EquipmentHolderDto, EquipmentHolderOutput>()
            .ForMember(x => x.FullName, opt => opt.MapFrom(x => $"{x.LastName} {x.FirstName}"));
        CreateMap<EquipmentTypeDto, EquipmentTypeOutput>();
        CreateMap<ActiveHolderDto, EquipmentHolderOutput>()
            .ForMember(x => x.FullName, opt => opt.MapFrom(x => $"{x.LastName} {x.FirstName}"));
        CreateMap<UseCases.Dto.EquipmentDto, EquipmentOutput>()
            .ForMember(x => x.InvoiceNumber, opt => opt.MapFrom(x => x.InvoiceNumber))
            .ForMember(x => x.EquipmentTypeId, opt => opt.MapFrom(x => x.TypeId))
            .ForMember(x => x.Location, opt => opt.MapFrom(x => x.Location.GetDescription()))
            .ForMember(x => x.PurchaseCurrency, opt => opt.MapFrom(x => x.PurchaseCurrency.GetDescription()))
            .ForMember(x => x.PurchasePriceAndCurrency, opt => opt.MapFrom(x => $"{x.PurchasePrice} {x.PurchaseCurrency.GetDescription()}"));
        CreateMap<UseCases.Dto.OData.EquipmentDto, EquipmentOutput>()
            .ForMember(dest => dest.PurchasePriceAndCurrency, opt => opt.MapFrom(src => $"{src.PurchasePrice} {src.PurchaseCurrency}"))
            .ForMember(dest => dest.EquipmentTypeId, opt => opt.MapFrom(src => src.TypeId));
        CreateMap<UserEquipmentDto, UserEquipmentOutput>()
            .ForMember(x => x.IssueDate, opt => opt.MapFrom(x => x.IssueDate.ToStringCustom(PortalConstants.Equipment.DefaultDateFormat)))
            .ForMember(x => x.ReturnDate, opt => opt.MapFrom(x => x.ReturnDate.ToStringCustom(PortalConstants.Equipment.DefaultDateFormat)))
            .ForMember(x => x.EquipmentTypeId, opt => opt.MapFrom(x => x.EquipmentTypeId));
    }
}
