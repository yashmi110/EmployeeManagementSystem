using MediatR;
using DepartmentService.Core.DTOs;

namespace DepartmentService.Core.CQRS.Commands;

public class CreateDepartmentCommand : IRequest<DepartmentDto>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
} 