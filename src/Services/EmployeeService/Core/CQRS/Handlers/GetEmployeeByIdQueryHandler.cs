using MediatR;
using EmployeeService.Core.CQRS.Queries;
using EmployeeService.Core.DTOs;
using EmployeeService.Core.Interfaces;

namespace EmployeeService.Core.CQRS.Handlers;

public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto?>
{
    private readonly IEmployeeService _employeeService;

    public GetEmployeeByIdQueryHandler(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public async Task<EmployeeDto?> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        return await _employeeService.GetEmployeeByIdAsync(request.Id);
    }
} 