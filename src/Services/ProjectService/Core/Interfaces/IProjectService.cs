using ProjectService.Core.DTOs;

namespace ProjectService.Core.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
    Task<ProjectDto?> GetProjectByIdAsync(int id);
    Task<ProjectDto> AddProjectAsync(CreateProjectDto createProjectDto);
    Task<ProjectDto?> UpdateProjectAsync(int id, UpdateProjectDto updateProjectDto);
    Task<bool> DeleteProjectAsync(int id);
} 