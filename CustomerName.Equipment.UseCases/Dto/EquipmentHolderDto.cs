﻿namespace CustomerName.Portal.Equipment.UseCases.Dto;

internal class EquipmentHolderDto
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? DepartmentId { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}
