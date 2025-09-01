using MediatR;
using ProjectService.Core.CQRS.Queries;
using ProjectService.Core.DTOs;
using ProjectService.Core.Interfaces;

namespace ProjectService.Core.CQRS.Handlers;

public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, IEnumerable<ProjectDto>>
{
    private readonly IProjectService _projectService;

    public GetAllProjectsQueryHandler(IProjectService projectService)
    {
        _projectService = projectService;
    }

    public async Task<IEnumerable<ProjectDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        return await _projectService.GetAllProjectsAsync();
    }
} 