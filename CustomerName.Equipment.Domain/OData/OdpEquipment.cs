namespace CustomerName.Portal.Equipment.Domain.OData;

public class OdpEquipment
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public required string Location { get; set; }
    public required string SerialNumber { get; set; }
    public decimal PurchasePrice { get; set; }
    public required string PurchaseCurrency { get; set; }
    public decimal PurchasePriceUsd { get; set; }
    public DateTime PurchaseDate { get; set; }
    public required string PurchasePlace { get; set; }
    public DateTime GuaranteeDate { get; set; }
    public required string Characteristics { get; set; }
    public string? Comment { get; set; }
    public string? InvoiceNumber { get; set; }

    public int ApproverId { get; set; }
    public string ApproverFullName { get; set; } = null!;

    public string EquipmentTypeId { get; set; } = null!;
    public string EquipmentTypeShortName { get; set; } = null!;
    public string EquipmentTypeFullName { get; set; } = null!;

    public int? ActiveHolderId { get; set; }
    public string? ActiveHolderFullName { get; set; }
    public string? ActiveHolderDepartmentId { get; set; }
    public DateTime? ActiveHolderIssueDate { get; set; }
    public DateTime? ActiveHolderReturnDate { get; set; }
}
