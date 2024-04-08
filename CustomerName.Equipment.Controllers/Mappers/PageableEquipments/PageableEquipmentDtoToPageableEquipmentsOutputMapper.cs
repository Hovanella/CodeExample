using CustomerName.Portal.Equipment.Controllers.Contracts.Output;
using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Framework.Utils.Extensions;

namespace CustomerName.Portal.Equipment.Controllers.Mappers.PageableEquipments;

internal interface IPageableEquipmentDtoToPageableEquipmentsOutputMapper
{
    PageableEquipmentsOutput Map(PageableEquipmentDto pageableEquipments);
}

internal class PageableEquipmentDtoToPageableEquipmentsOutputMapper : IPageableEquipmentDtoToPageableEquipmentsOutputMapper
{
    public PageableEquipmentsOutput Map(PageableEquipmentDto pageableEquipments)
    {
        var equipmentItemsOutput = pageableEquipments
            .Items.Select(x => {
            var locationDescription = Enum.Parse<EquipmentLocationType>(x.Location).GetDescription();
            var purchaseCurrencyDescription = Enum.Parse<MoneyCurrencyType>(x.PurchaseCurrency,ignoreCase: true).GetDescription();

            return new EquipmentOutput
            {
                Id = x.Id,
                Name = x.Name,
                EquipmentTypeId = x.EquipmentTypeId,
                Location = locationDescription,
                SerialNumber = x.SerialNumber,
                PurchasePrice = x.PurchasePrice,
                PurchaseCurrency = purchaseCurrencyDescription,
                PurchasePriceAndCurrency =
                    $"{x.PurchasePrice} {purchaseCurrencyDescription}",
                PurchasePriceUsd = x.PurchasePriceUsd,
                PurchaseDate = x.PurchaseDate,
                PurchasePlace = x.PurchasePlace,
                GuaranteeDate = x.GuaranteeDate,
                Characteristics = x.Characteristics,
                Comment = x.Comment,
                InvoiceNumber = x.InvoiceNumber,
                Approver = new ApproverOutput
                {
                    Id = x.ApproverId, FullName = x.ApproverFullName
                },
                ActiveHolder = x is { ActiveHolderId: not null, ActiveHolderIssueDate: not null }
                    ? new EquipmentHolderOutput
                    {
                        Id = x.ActiveHolderId.Value,
                        FullName = x.ActiveHolderFullName!,
                        DepartmentId = x.ActiveHolderDepartmentId,
                        IssueDate = x.ActiveHolderIssueDate.Value,
                        ReturnDate = x.ActiveHolderReturnDate,
                    }
                    : null,
                Type = new EquipmentTypeOutput(x.EquipmentTypeId, x.EquipmentTypeShortName, x.EquipmentTypeFullName)
            };
        }).ToList();

        return new PageableEquipmentsOutput
        {
            Items = equipmentItemsOutput,
            Page = pageableEquipments.Page,
            PageSize = pageableEquipments.PageSize,
            Total = pageableEquipments.Total,
            FilterValues = new AvailableFilterOptionsOutput
            {
                HasAvailable = pageableEquipments.FilterValues.HasAvailable,
                EquipmentLocations = pageableEquipments.FilterValues.EquipmentLocations
                    .Cast<int>()
                    .ToList(),
                EquipmentTypeIds = pageableEquipments.FilterValues.EquipmentTypeIds,
                Approvers = pageableEquipments
                    .FilterValues
                    .Approvers
                    .Select(x => new UserFilterOutput
                {
                    Id = x.Id, FirstName = x.FirstName, LastName = x.LastName
                }).ToList(),
                Users = pageableEquipments
                    .FilterValues
                    .Users
                    .Select(x => new UserFilterOutput
                {
                    Id = x.Id, FirstName = x.FirstName, LastName = x.LastName
                }).ToList()
            }
        };

    }
}
