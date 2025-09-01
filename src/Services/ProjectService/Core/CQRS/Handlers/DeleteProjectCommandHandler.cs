using MediatR;
using ProjectService.Core.CQRS.Commands;
using ProjectService.Core.Interfaces;

namespace ProjectService.Core.CQRS.Handlers;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, bool>
{
    private readonly IProjectService _projectService;

    public DeleteProjectCommandHandler(IProjectService projectService)
    {
        _projectService = projectService;
    }

    public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        return await _projectService.DeleteProjectAsync(request.Id);
    }
} 