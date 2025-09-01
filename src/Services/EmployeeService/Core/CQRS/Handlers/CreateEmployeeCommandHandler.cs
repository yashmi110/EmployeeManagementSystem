using MediatR;
using EmployeeService.Core.CQRS.Commands;
using EmployeeService.Core.DTOs;
using EmployeeService.Core.Interfaces;

namespace EmployeeService.Core.CQRS.Handlers;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
{
    private readonly IEmployeeService _employeeService;

    public CreateEmployeeCommandHandler(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public async Task<EmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var createEmployeeDto = new CreateEmployeeDto
        {
            Name = request.Name,
            Age = request.Age,
            Department = request.Department,
            Salary = (double)request.Salary,
            ManagerId = request.ManagerId
        };

        return await _employeeService.AddEmployeeAsync(createEmployeeDto);
    }
} 