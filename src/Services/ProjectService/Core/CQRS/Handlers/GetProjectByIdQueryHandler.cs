using MediatR;
using ProjectService.Core.CQRS.Queries;
using ProjectService.Core.DTOs;
using ProjectService.Core.Interfaces;

namespace ProjectService.Core.CQRS.Handlers;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto?>
{
    private readonly IProjectService _projectService;

    public GetProjectByIdQueryHandler(IProjectService projectService)
    {
        _projectService = projectService;
    }

    public async Task<ProjectDto?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        return await _projectService.GetProjectByIdAsync(request.Id);
    }
} 