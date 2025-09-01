using MediatR;
using DepartmentService.Core.DTOs;

namespace DepartmentService.Core.CQRS.Queries;

public class GetAllDepartmentsQuery : IRequest<IEnumerable<DepartmentDto>>
{
} 