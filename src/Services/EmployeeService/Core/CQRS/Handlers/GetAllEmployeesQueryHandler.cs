using MediatR;
using EmployeeService.Core.CQRS.Queries;
using EmployeeService.Core.DTOs;
using EmployeeService.Core.Interfaces;

namespace EmployeeService.Core.CQRS.Handlers;

public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, IEnumerable<EmployeeDto>>
{
    private readonly IEmployeeService _employeeService;

    public GetAllEmployeesQueryHandler(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public async Task<IEnumerable<EmployeeDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        return await _employeeService.GetAllEmployeesAsync();
    }
} 