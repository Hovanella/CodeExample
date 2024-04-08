namespace CustomerName.Portal.Equipment.UseCases.Dto.OData;

internal class EquipmentDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string SerialNumber { get; set; }
    public required string TypeId { get; set; }
    public double PurchasePrice { get; set; }
    public required string PurchaseCurrency { get; set; }
    public double PurchasePriceUsd { get; set; }
    public required string Location { get; set; }
    public string? InvoiceNumber { get; set; }
    public DateTime PurchaseDate { get; set; }
    public required string PurchasePlace { get; set; }
    public DateTime GuaranteeDate { get; set; }
    public required string Characteristics { get; set; }
    public string? Comment { get; set; }
    public ActiveHolderDto? ActiveHolder { get; set; }
    public ApproverDto? Approver { get; set; }
    public EquipmentTypeDto? Type { get; set; }
}
