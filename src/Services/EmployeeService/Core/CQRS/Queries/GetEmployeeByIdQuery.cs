using MediatR;
using EmployeeService.Core.DTOs;

namespace EmployeeService.Core.CQRS.Queries;

public class GetEmployeeByIdQuery : IRequest<EmployeeDto?>
{
    public int Id { get; set; }
} 