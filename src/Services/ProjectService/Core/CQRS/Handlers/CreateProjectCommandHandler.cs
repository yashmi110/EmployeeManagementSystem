using MediatR;
using ProjectService.Core.CQRS.Commands;
using ProjectService.Core.DTOs;
using ProjectService.Core.Interfaces;

namespace ProjectService.Core.CQRS.Handlers;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
{
    private readonly IProjectService _projectService;

    public CreateProjectCommandHandler(IProjectService projectService)
    {
        _projectService = projectService;
    }

    public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var createProjectDto = new CreateProjectDto
        {
            Name = request.Name,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Status = request.Status,
            Budget = (double)request.Budget, // Cast to double
            ManagerId = request.ManagerId
        };

        return await _projectService.AddProjectAsync(createProjectDto);
    }
} 