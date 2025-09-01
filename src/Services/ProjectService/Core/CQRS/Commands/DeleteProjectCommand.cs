using MediatR;

namespace ProjectService.Core.CQRS.Commands;

public class DeleteProjectCommand : IRequest<bool>
{
    public int Id { get; set; }
} 