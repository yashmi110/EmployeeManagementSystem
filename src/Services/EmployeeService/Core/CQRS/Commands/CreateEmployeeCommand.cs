using MediatR;
using EmployeeService.Core.DTOs;

namespace EmployeeService.Core.CQRS.Commands;

public class CreateEmployeeCommand : IRequest<EmployeeDto>
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Department { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public DateTime HireDate { get; set; }
    public int? ManagerId { get; set; }
    
    // Manager-specific properties
    public string? ManagementLevel { get; set; }
    public int? TeamSize { get; set; }
} 