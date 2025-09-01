using DepartmentService.Core.DTOs;
using DepartmentService.Core.CQRS.Commands;
using DepartmentService.Core.CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DepartmentService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DepartmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/departments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments()
    {
        var query = new GetAllDepartmentsQuery();
        var departments = await _mediator.Send(query);
        return Ok(departments);
    }

    // GET: api/departments/5
    [HttpGet("{id}")]
    public async Task<ActionResult<DepartmentDto>> GetDepartment(int id)
    {
        var query = new GetDepartmentByIdQuery { Id = id };
        var department = await _mediator.Send(query);

        if (department == null)
        {
            return NotFound($"Department with ID {id} not found.");
        }

        return Ok(department);
    }

    // POST: api/departments
    [HttpPost]
    public async Task<ActionResult<DepartmentDto>> CreateDepartment(CreateDepartmentCommand command)
    {
        var department = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetDepartment), new { id = department.Id }, department);
    }

    // PUT: api/departments/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDepartment(int id, UpdateDepartmentCommand command)
    {
        command.Id = id; // Ensure the ID from the route is used
        var department = await _mediator.Send(command);

        if (department == null)
        {
            return NotFound($"Department with ID {id} not found.");
        }

        return Ok(department);
    }

    // DELETE: api/departments/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        var command = new DeleteDepartmentCommand { Id = id };
        var success = await _mediator.Send(command);

        if (!success)
        {
            return NotFound($"Department with ID {id} not found.");
        }

        return NoContent();
    }
} 