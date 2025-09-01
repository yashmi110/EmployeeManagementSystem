namespace ProjectService.Core.DTOs;

public class ProjectDto
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

public class CreateProjectDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public double Budget { get; set; }
    public int ManagerId { get; set; }
}

public class UpdateProjectDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public double Budget { get; set; }
    public int ManagerId { get; set; }
}

public class ProjectEmployeeDto
{
    public int ProjectId { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime AssignedDate { get; set; }
    public DateTime? UnassignedDate { get; set; }
    public bool IsActive { get; set; }
}

public class AssignEmployeeDto
{
    public int EmployeeId { get; set; }
    public string Role { get; set; } = string.Empty;
}

public class ProjectDetailsDto : ProjectDto
{
    public List<ProjectEmployeeDto> Employees { get; set; } = new List<ProjectEmployeeDto>();
} 