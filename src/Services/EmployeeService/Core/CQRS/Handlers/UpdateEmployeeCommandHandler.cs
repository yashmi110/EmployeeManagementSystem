using MediatR;
using EmployeeService.Core.CQRS.Commands;
using EmployeeService.Core.DTOs;
using EmployeeService.Core.Interfaces;

namespace EmployeeService.Core.CQRS.Handlers;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeDto?>
{
    private readonly IEmployeeService _employeeService;

    public UpdateEmployeeCommandHandler(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public async Task<EmployeeDto?> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var updateEmployeeDto = new UpdateEmployeeDto
        {
            Name = request.Name,
            Age = request.Age,
            Department = request.Department,
            Salary = (double)request.Salary,
            ManagerId = request.ManagerId,
            IsActive = request.IsActive
        };

        return await _employeeService.UpdateEmployeeAsync(request.Id, updateEmployeeDto);
    }
} 