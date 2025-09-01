using MediatR;

namespace EmployeeService.Core.CQRS.Commands;

public class DeleteEmployeeCommand : IRequest<bool>
{
    public int Id { get; set; }
} 