using ProjectService.Core;
using ProjectService.Core.DTOs;
using ProjectService.Core.Interfaces;
using ProjectService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ProjectService.Infrastructure.Repositories;

public class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _context;

    public ProjectService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
    {
        var projects = await _context.Projects
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Status = p.Status,
                Budget = p.Budget,
                ManagerId = p.ManagerId,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            })
            .ToListAsync();

        return projects;
    }

    public async Task<ProjectDto?> GetProjectByIdAsync(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
            return null;

        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Status = project.Status,
            Budget = project.Budget,
            ManagerId = project.ManagerId,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt
        };
    }

    public async Task<ProjectDto> AddProjectAsync(CreateProjectDto createProjectDto)
    {
        var project = new Project
        {
            Name = createProjectDto.Name,
            Description = createProjectDto.Description,
            StartDate = createProjectDto.StartDate,
            EndDate = createProjectDto.EndDate,
            Status = createProjectDto.Status,
            Budget = createProjectDto.Budget,
            ManagerId = createProjectDto.ManagerId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Status = project.Status,
            Budget = project.Budget,
            ManagerId = project.ManagerId,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt
        };
    }

    public async Task<ProjectDto?> UpdateProjectAsync(int id, UpdateProjectDto updateProjectDto)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
            return null;

        project.Name = updateProjectDto.Name;
        project.Description = updateProjectDto.Description;
        project.StartDate = updateProjectDto.StartDate;
        project.EndDate = updateProjectDto.EndDate;
        project.Status = updateProjectDto.Status;
        project.Budget = updateProjectDto.Budget;
        project.ManagerId = updateProjectDto.ManagerId;
        project.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Status = project.Status,
            Budget = project.Budget,
            ManagerId = project.ManagerId,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt
        };
    }

    public async Task<bool> DeleteProjectAsync(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
            return false;

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        return true;
    }
} 