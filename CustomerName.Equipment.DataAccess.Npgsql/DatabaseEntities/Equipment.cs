using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Framework.Entities;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities;

internal class Equipment : AuditableEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public EquipmentLocationType Location { get; set; }
    public string? SerialNumber { get; set; }
    public decimal PurchasePrice { get; set; }
    public MoneyCurrencyType PurchaseCurrency { get; set; }
    public decimal PurchasePriceUsd { get; set; }
    public DateTime PurchaseDate { get; set; }
    public string? PurchasePlace { get; set; }
    public DateTime GuaranteeDate { get; set; }
    public string? Characteristics { get; set; }
    public string? Comment { get; set; }
    public string? InvoiceNumber { get; set; }

    public ICollection<EquipmentAssign> Assigns { get; set; } = new List<EquipmentAssign>();

    public int? ApproverId { get; set; }
    public EquipmentUser? Approver { get; set; }

    public string? TypeId { get; set; }
    public EquipmentType? Type { get; set; }
}
