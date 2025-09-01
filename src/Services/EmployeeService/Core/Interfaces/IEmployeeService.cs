using EmployeeService.Core.DTOs;

namespace EmployeeService.Core.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
    Task<EmployeeDto?> GetEmployeeByIdAsync(int id);
    Task<EmployeeDto> AddEmployeeAsync(CreateEmployeeDto createEmployeeDto);
    Task<EmployeeDto?> UpdateEmployeeAsync(int id, UpdateEmployeeDto updateEmployeeDto);
    Task<bool> DeleteEmployeeAsync(int id);
    Task<IEnumerable<EmployeeDto>> GetEmployeesByDepartmentAsync(string department);
    Task<IEnumerable<ManagerDto>> GetAllManagersAsync();
    Task<double> GetAverageAgeAsync();
    Task<double> GetAverageSalaryAsync();
    Task<int> GetEmployeeCountAsync();
    Task<bool> ExistsAsync(int id);
} 