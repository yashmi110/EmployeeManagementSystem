using MediatR;
using EmployeeService.Core.DTOs;

namespace EmployeeService.Core.CQRS.Queries;

public class GetAllEmployeesQuery : IRequest<IEnumerable<EmployeeDto>>
{
} 