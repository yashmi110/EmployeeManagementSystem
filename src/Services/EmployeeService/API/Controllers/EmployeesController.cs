using EmployeeService.Core.DTOs;
using EmployeeService.Core.CQRS.Commands;
using EmployeeService.Core.CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/employees
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
    {
        try
        {
            var query = new GetAllEmployeesQuery();
            var employees = await _mediator.Send(query);
            return Ok(employees);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // GET: api/employees/5
    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
    {
        try
        {
            var query = new GetEmployeeByIdQuery { Id = id };
            var employee = await _mediator.Send(query);

            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }

            return Ok(employee);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // POST: api/employees
    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> CreateEmployee(CreateEmployeeCommand command)
    {
        try
        {
            var employee = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // PUT: api/employees/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, UpdateEmployeeCommand command)
    {
        try
        {
            command.Id = id; // Ensure the ID from the route is used
            var employee = await _mediator.Send(command);

            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }

            return Ok(employee);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // DELETE: api/employees/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        try
        {
            var command = new DeleteEmployeeCommand { Id = id };
            var success = await _mediator.Send(command);

            if (!success)
            {
                return NotFound($"Employee with ID {id} not found.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
} 