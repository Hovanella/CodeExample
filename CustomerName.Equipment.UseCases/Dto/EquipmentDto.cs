using CustomerName.Portal.Framework.Core;

namespace CustomerName.Portal.Equipment.UseCases.Dto;

internal class EquipmentDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string SerialNumber { get; set; }
    public string TypeId { get; set; }
    public decimal PurchasePrice { get; set; }
    public MoneyCurrencyType PurchaseCurrency { get; set; }
    public decimal PurchasePriceUsd { get; set; }
    public EquipmentLocationType Location { get; set; }
    public DateTime PurchaseDate { get; set; }
    public required string PurchasePlace { get; set; }
    public DateTime GuaranteeDate { get; set; }
    public required string Characteristics { get; set; }
    public string? Comment { get; set; }
    public string? InvoiceNumber { get; set; }

    public ApproverDto? Approver { get; set; }
    public EquipmentHolderDto? ActiveHolder { get; set; }
    public EquipmentTypeDto? Type { get; set; }
}
