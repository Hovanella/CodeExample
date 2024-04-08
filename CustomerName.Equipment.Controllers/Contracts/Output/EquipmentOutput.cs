namespace CustomerName.Portal.Equipment.Controllers.Contracts.Output;

public class EquipmentOutput
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Location { get; set; }
    public required string SerialNumber { get; set; }
    public decimal PurchasePrice { get; set; }
    public required string PurchaseCurrency { get; set; }
    public required string PurchasePriceAndCurrency { get; set; }
    public decimal PurchasePriceUsd { get; set; }
    public DateTime PurchaseDate { get; set; }
    public required string PurchasePlace { get; set; }
    public DateTime GuaranteeDate { get; set; }
    public required string Characteristics { get; set; }
    public string? Comment { get; set; }
    public string? InvoiceNumber { get; set; }

    public required string EquipmentTypeId { get; set; }

    public required ApproverOutput Approver { get; set; }
    public EquipmentHolderOutput? ActiveHolder { get; set; }
    public EquipmentTypeOutput Type { get; set; }
}
