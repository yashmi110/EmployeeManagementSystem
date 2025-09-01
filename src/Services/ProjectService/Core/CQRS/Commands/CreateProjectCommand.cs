using MediatR;
using ProjectService.Core.DTOs;

namespace ProjectService.Core.CQRS.Commands;

public class CreateProjectCommand : IRequest<ProjectDto>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal Budget { get; set; }
    public int ManagerId { get; set; }
} 