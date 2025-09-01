using MediatR;
using ProjectService.Core.CQRS.Commands;
using ProjectService.Core.DTOs;
using ProjectService.Core.Interfaces;

namespace ProjectService.Core.CQRS.Handlers;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, ProjectDto?>
{
    private readonly IProjectService _projectService;

    public UpdateProjectCommandHandler(IProjectService projectService)
    {
        _projectService = projectService;
    }

    public async Task<ProjectDto?> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var updateProjectDto = new UpdateProjectDto
        {
            Name = request.Name,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Status = request.Status,
            Budget = (double)request.Budget, // Cast to double
            ManagerId = request.ManagerId
        };

        return await _projectService.UpdateProjectAsync(request.Id, updateProjectDto);
    }
} 