namespace CustomerName.Portal.Equipment.Controllers.Contracts.Input;

public class ActiveHolderInput
{
    public int UserId { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}
