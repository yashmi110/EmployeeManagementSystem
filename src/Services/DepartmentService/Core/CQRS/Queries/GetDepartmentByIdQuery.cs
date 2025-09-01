using MediatR;
using DepartmentService.Core.DTOs;

namespace DepartmentService.Core.CQRS.Queries;

public class GetDepartmentByIdQuery : IRequest<DepartmentDto?>
{
    public int Id { get; set; }
} 