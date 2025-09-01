namespace EmployeeService.Core;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Department { get; set; } = string.Empty;
    public double Salary { get; set; }
    public DateTime HireDate { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    public int? ManagerId { get; set; }
    public Employee? Manager { get; set; }
    public ICollection<Employee> TeamMembers { get; set; } = new List<Employee>();
    public int TeamSize => TeamMembers?.Count ?? 0;
}

public class Manager : Employee
{
    public new int TeamSize { get; set; }
    public string ManagementLevel { get; set; } = string.Empty;
} 