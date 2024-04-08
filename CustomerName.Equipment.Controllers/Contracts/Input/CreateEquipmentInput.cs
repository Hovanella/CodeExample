using CustomerName.Portal.Framework.Core;

namespace CustomerName.Portal.Equipment.Controllers.Contracts.Input;

public class CreateEquipmentInput
{
    public required string Name { get; set; }
    public string EquipmentTypeId { get; set; }
    public required string SerialNumber { get; set; }
    public decimal PurchasePrice { get; set; }
    public MoneyCurrencyType PurchaseCurrency { get; set; }
    public decimal PurchasePriceUsd { get; set; }
    public DateTime PurchaseDate { get; set; }
    public required string PurchasePlace { get; set; }
    public DateTime GuaranteeDate { get; set; }
    public required string Characteristics { get; set; }
    public int ApproverId { get; set; }
    public EquipmentLocationType Location { get; set; }
    public string? Comment { get; set; }
    public required string InvoiceNumber { get; set; }
}
