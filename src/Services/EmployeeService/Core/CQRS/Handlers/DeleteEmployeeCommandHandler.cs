using MediatR;
using EmployeeService.Core.CQRS.Commands;
using EmployeeService.Core.Interfaces;

namespace EmployeeService.Core.CQRS.Handlers;

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, bool>
{
    private readonly IEmployeeService _employeeService;

    public DeleteEmployeeCommandHandler(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        return await _employeeService.DeleteEmployeeAsync(request.Id);
    }
} 