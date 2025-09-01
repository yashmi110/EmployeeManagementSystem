using MediatR;

namespace DepartmentService.Core.CQRS.Commands;

public class DeleteDepartmentCommand : IRequest<bool>
{
    public int Id { get; set; }
} 