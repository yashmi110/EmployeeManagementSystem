namespace ProjectService.Core;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public double Budget { get; set; }
    public int ManagerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class ProjectEmployee
{
    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;
    public int EmployeeId { get; set; }
    public string Role { get; set; } = string.Empty; // Developer, Lead, Tester, etc.
    public DateTime AssignedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UnassignedDate { get; set; }
    public bool IsActive { get; set; } = true;
} 