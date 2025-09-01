using Shared.Messaging.Contracts;

namespace Shared.Messaging.Contracts;

public class EmployeeCreatedEvent : BaseMessage
{
    public int EmployeeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Department { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public int? ManagerId { get; set; }
}

public class EmployeeUpdatedEvent : BaseMessage
{
    public int EmployeeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Department { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public int? ManagerId { get; set; }
    public bool IsActive { get; set; }
}

public class EmployeeDeletedEvent : BaseMessage
{
    public int EmployeeId { get; set; }
}

public class EmployeeDepartmentChangedEvent : BaseMessage
{
    public int EmployeeId { get; set; }
    public string OldDepartment { get; set; } = string.Empty;
    public string NewDepartment { get; set; } = string.Empty;
} 