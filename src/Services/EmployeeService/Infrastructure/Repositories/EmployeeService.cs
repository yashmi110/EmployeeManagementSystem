using EmployeeService.Core;
using EmployeeService.Core.DTOs;
using EmployeeService.Core.Exceptions;
using EmployeeService.Core.Extensions;
using EmployeeService.Core.Interfaces;
using EmployeeService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EmployeeService.Infrastructure.Repositories;

public class EmployeeService : IEmployeeService
{
    private readonly ApplicationDbContext _context;

    public EmployeeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
    {
        return await _context.Employees
            .Where(e => e.IsActive)
            .Include(e => e.Manager)
            .Select(e => new EmployeeDto
            {
                Id = e.Id,
                Name = e.Name.FormatName(),
                Age = e.Age,
                Department = e.Department,
                Salary = e.Salary,
                HireDate = e.HireDate,
                IsActive = e.IsActive,
                ManagerId = e.ManagerId,
                ManagerName = e.Manager != null ? e.Manager.Name.FormatName() : null,
                TeamSize = e.TeamSize
            })
            .ToListAsync();
    }

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
    {
        var employee = await _context.Employees
            .Where(e => e.Id == id && e.IsActive)
            .Include(e => e.Manager)
            .Select(e => new EmployeeDto
            {
                Id = e.Id,
                Name = e.Name.FormatName(),
                Age = e.Age,
                Department = e.Department,
                Salary = e.Salary,
                HireDate = e.HireDate,
                IsActive = e.IsActive,
                ManagerId = e.ManagerId,
                ManagerName = e.Manager != null ? e.Manager.Name.FormatName() : null,
                TeamSize = e.TeamSize
            })
            .FirstOrDefaultAsync();

        return employee;
    }

    public async Task<EmployeeDto> AddEmployeeAsync(CreateEmployeeDto createEmployeeDto)
    {
        if (createEmployeeDto.Age < 0)
            throw new ArgumentException("Age cannot be negative.");

        if (string.IsNullOrWhiteSpace(createEmployeeDto.Name))
            throw new ArgumentException("Name is required.");

        var employee = new Employee
        {
            Name = createEmployeeDto.Name,
            Age = createEmployeeDto.Age,
            Department = createEmployeeDto.Department,
            Salary = createEmployeeDto.Salary,
            ManagerId = createEmployeeDto.ManagerId,
            HireDate = DateTime.UtcNow,
            IsActive = true
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return new EmployeeDto
        {
            Id = employee.Id,
            Name = employee.Name.FormatName(),
            Age = employee.Age,
            Department = employee.Department,
            Salary = employee.Salary,
            HireDate = employee.HireDate,
            IsActive = employee.IsActive,
            ManagerId = employee.ManagerId,
            TeamSize = employee.TeamSize
        };
    }

    public async Task<EmployeeDto?> UpdateEmployeeAsync(int id, UpdateEmployeeDto updateEmployeeDto)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == id && e.IsActive);

        if (employee == null)
            return null;

        if (updateEmployeeDto.Age < 0)
            throw new ArgumentException("Age cannot be negative.");

        employee.Name = updateEmployeeDto.Name;
        employee.Age = updateEmployeeDto.Age;
        employee.Department = updateEmployeeDto.Department;
        employee.Salary = updateEmployeeDto.Salary;
        employee.IsActive = updateEmployeeDto.IsActive;
        employee.ManagerId = updateEmployeeDto.ManagerId;

        await _context.SaveChangesAsync();

        return new EmployeeDto
        {
            Id = employee.Id,
            Name = employee.Name.FormatName(),
            Age = employee.Age,
            Department = employee.Department,
            Salary = employee.Salary,
            HireDate = employee.HireDate,
            IsActive = employee.IsActive,
            ManagerId = employee.ManagerId,
            TeamSize = employee.TeamSize
        };
    }

    public async Task<bool> DeleteEmployeeAsync(int id)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == id && e.IsActive);

        if (employee == null)
            return false;

        employee.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<EmployeeDto>> GetEmployeesByDepartmentAsync(string department)
    {
        var employees = await GetAllEmployeesAsync();
        return employees.FilterByDepartment(department);
    }

    public async Task<IEnumerable<ManagerDto>> GetAllManagersAsync()
    {
        return await _context.Employees
            .Where(e => e.IsActive && e.ManagerId == null)
            .Include(e => e.TeamMembers)
            .Select(e => new ManagerDto
            {
                Id = e.Id,
                Name = e.Name.FormatName(),
                Age = e.Age,
                Department = e.Department,
                Salary = e.Salary,
                HireDate = e.HireDate,
                IsActive = e.IsActive,
                ManagerId = e.ManagerId,
                TeamSize = e.TeamSize,
                ManagementLevel = "Team Lead",
                TeamMembers = e.TeamMembers.Select(tm => new EmployeeDto
                {
                    Id = tm.Id,
                    Name = tm.Name.FormatName(),
                    Age = tm.Age,
                    Department = tm.Department,
                    Salary = tm.Salary,
                    HireDate = tm.HireDate,
                    IsActive = tm.IsActive,
                    ManagerId = tm.ManagerId,
                    TeamSize = tm.TeamSize
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task<double> GetAverageAgeAsync()
    {
        var employees = await GetAllEmployeesAsync();
        return employees.CalculateAverageAge();
    }

    public async Task<double> GetAverageSalaryAsync()
    {
        var employees = await GetAllEmployeesAsync();
        return employees.CalculateAverageSalary();
    }

    public async Task<int> GetEmployeeCountAsync()
    {
        return await _context.Employees
            .Where(e => e.IsActive)
            .CountAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Employees
            .AnyAsync(e => e.Id == id && e.IsActive);
    }
} 