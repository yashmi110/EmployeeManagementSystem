using DepartmentService.Core.DTOs;

namespace DepartmentService.Core.Interfaces;

public interface IDepartmentRepository
{
    Task<IEnumerable<DepartmentDto>> GetAllAsync();
    Task<DepartmentDto?> GetByIdAsync(int id);
    Task<DepartmentDto> CreateAsync(CreateDepartmentDto createDepartmentDto);
    Task<DepartmentDto?> UpdateAsync(int id, UpdateDepartmentDto updateDepartmentDto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
} 