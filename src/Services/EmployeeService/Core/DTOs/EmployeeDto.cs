namespace EmployeeService.Core.DTOs;

public class EmployeeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Department { get; set; } = string.Empty;
    public double Salary { get; set; }
    public DateTime HireDate { get; set; }
    public bool IsActive { get; set; }
    public int? ManagerId { get; set; }
    public string? ManagerName { get; set; }
    public int TeamSize { get; set; }
}

public class CreateEmployeeDto
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Department { get; set; } = string.Empty;
    public double Salary { get; set; }
    public int? ManagerId { get; set; }
}

public class UpdateEmployeeDto
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Department { get; set; } = string.Empty;
    public double Salary { get; set; }
    public bool IsActive { get; set; } = true;
    public int? ManagerId { get; set; }
}

public class ManagerDto : EmployeeDto
{
    public string ManagementLevel { get; set; } = string.Empty;
    public List<EmployeeDto> TeamMembers { get; set; } = new List<EmployeeDto>();
} 