using CustomerName.Portal.Framework.Core;

namespace CustomerName.Portal.Equipment.Domain;

internal class Equipment
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public EquipmentLocationType Location { get; set; }
    public required string SerialNumber { get; set; }
    public decimal PurchasePrice { get; set; }
    public MoneyCurrencyType PurchaseCurrency { get; set; }
    public decimal PurchasePriceUsd { get; set; }
    public DateTime PurchaseDate { get; set; }
    public required string PurchasePlace { get; set; }
    public DateTime GuaranteeDate { get; set; }
    public required string Characteristics { get; set; }
    public string? Comment { get; set; }
    public string? InvoiceNumber { get; set; }

    public int ApproverId { get; set; }
    public EquipmentUser? Approver { get; set; }

    public string TypeId { get; set; }
    public EquipmentType? Type { get; set; }

    public List<EquipmentAssign> Assigns { get; set; } = [];

    public EquipmentAssign? GetActiveAssign(DateTime currentTime)
    {
        return Assigns.Where(x =>
            !x.IsDeleted &&
            (x.ReturnDate == null || x.ReturnDate >= currentTime))
            .MinBy(x => x.IssueDate);
    }
}
