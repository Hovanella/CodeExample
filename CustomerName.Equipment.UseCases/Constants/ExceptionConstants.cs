namespace CustomerName.Portal.Equipment.UseCases.Constants;

internal static class ExceptionConstants
{
    public const string ApproverDoesNotExist = "User with provided approver ID does not exist";
    public const string ApproverDoesNotHavePermission = "Provided user does not have permission to approve";
    public const string EquipmentNotFound = "Equipment was not found";
    public const string UserNotFound = "User was not found";
    public const string EquipmentAlreadyAssigned = "This equipment has already been assigned";
    public const string HeadCannotSeeOtherDepartmentEquipment = "The head of the department cannot see the equipment assigned to the user from another department";
    public const string YouDoNotHavePermission = "You don't have permission";
    public const string ReturnDateLowerThanIssueDate = "The return date must be equal or later than the issue date";
    public const string EquipmentTypeNotFound = "Equipment type was not found";
    public const string EquipmentReportAssemblyDateNotFound = "Equipment report doesn't have creation date property";
    public const string EquipmentReportSerialNumberNotFound = "Equipment report doesn't have hardware serial number property";
    public const string EquipmentReportNotFound = "Equipment report hasn't been found";
    public const string EquipmentReportRelevancePeriodsNotFound = "Equipment report relevance periods have't been found";
    public const string EquipmentWasNeverAssigned = "This equipment was never assigned";
    public const string SeveralNullDatesForEquipment = "There are several null return dates for chosen equipment";
    public const string ExistingReturnDateForLastAssignment = "There is existing return date for the last assignment!";
    public const string ExistingSerialNumber = "An equipment with the same serial number already exists";
}
