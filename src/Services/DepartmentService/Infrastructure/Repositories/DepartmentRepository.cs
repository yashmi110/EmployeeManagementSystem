using DepartmentService.Core;
using DepartmentService.Core.DTOs;
using DepartmentService.Core.Interfaces;
using DepartmentService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DepartmentService.Infrastructure.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly ApplicationDbContext _context;

    public DepartmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        return await _context.Departments
            .Where(d => d.IsActive)
            .Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                CreatedDate = d.CreatedDate,
                UpdatedDate = d.UpdatedDate,
                IsActive = d.IsActive
            })
            .ToListAsync();
    }

    public async Task<DepartmentDto?> GetByIdAsync(int id)
    {
        var department = await _context.Departments
            .Where(d => d.Id == id && d.IsActive)
            .Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                CreatedDate = d.CreatedDate,
                UpdatedDate = d.UpdatedDate,
                IsActive = d.IsActive
            })
            .FirstOrDefaultAsync();

        return department;
    }

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto createDepartmentDto)
    {
        var department = new Department
        {
            Name = createDepartmentDto.Name,
            Description = createDepartmentDto.Description,
            CreatedDate = DateTime.UtcNow,
            IsActive = true
        };

        _context.Departments.Add(department);
        await _context.SaveChangesAsync();

        return new DepartmentDto
        {
            Id = department.Id,
            Name = department.Name,
            Description = department.Description,
            CreatedDate = department.CreatedDate,
            UpdatedDate = department.UpdatedDate,
            IsActive = department.IsActive
        };
    }

    public async Task<DepartmentDto?> UpdateAsync(int id, UpdateDepartmentDto updateDepartmentDto)
    {
        var department = await _context.Departments
            .FirstOrDefaultAsync(d => d.Id == id && d.IsActive);

        if (department == null)
            return null;

        department.Name = updateDepartmentDto.Name;
        department.Description = updateDepartmentDto.Description;
        department.IsActive = updateDepartmentDto.IsActive;
        department.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new DepartmentDto
        {
            Id = department.Id,
            Name = department.Name,
            Description = department.Description,
            CreatedDate = department.CreatedDate,
            UpdatedDate = department.UpdatedDate,
            IsActive = department.IsActive
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var department = await _context.Departments
            .FirstOrDefaultAsync(d => d.Id == id && d.IsActive);

        if (department == null)
            return false;

        department.IsActive = false;
        department.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Departments
            .AnyAsync(d => d.Id == id && d.IsActive);
    }
} 