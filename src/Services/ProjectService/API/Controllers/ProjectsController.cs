using ProjectService.Core.DTOs;
using ProjectService.Core.CQRS.Commands;
using ProjectService.Core.CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ProjectService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/projects
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
    {
        try
        {
            var query = new GetAllProjectsQuery();
            var projects = await _mediator.Send(query);
            return Ok(projects);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // GET: api/projects/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDto>> GetProject(int id)
    {
        try
        {
            var query = new GetProjectByIdQuery { Id = id };
            var project = await _mediator.Send(query);

            if (project == null)
            {
                return NotFound($"Project with ID {id} not found.");
            }

            return Ok(project);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // POST: api/projects
    [HttpPost]
    public async Task<ActionResult<ProjectDto>> CreateProject(CreateProjectCommand command)
    {
        try
        {
            var project = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // PUT: api/projects/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(int id, UpdateProjectCommand command)
    {
        try
        {
            command.Id = id; // Ensure the ID from the route is used
            var project = await _mediator.Send(command);

            if (project == null)
            {
                return NotFound($"Project with ID {id} not found.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // DELETE: api/projects/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        try
        {
            var command = new DeleteProjectCommand { Id = id };
            var success = await _mediator.Send(command);

            if (!success)
            {
                return NotFound($"Project with ID {id} not found.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
} 