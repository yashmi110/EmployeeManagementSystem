using EmployeeService.Core.DTOs;
using System.Globalization;

namespace EmployeeService.Core.Extensions;

public static class EmployeeExtensions
{
    public static string FormatName(this string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return string.Empty;

        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());
    }

    public static IEnumerable<EmployeeDto> FilterByDepartment(this IEnumerable<EmployeeDto> employees, string department)
    {
        return employees.Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase));
    }

    public static double CalculateAverageAge(this IEnumerable<EmployeeDto> employees)
    {
        if (!employees.Any())
            return 0;

        return employees.Average(e => e.Age);
    }

    public static double CalculateAverageSalary(this IEnumerable<EmployeeDto> employees)
    {
        if (!employees.Any())
            return 0;

        return employees.Average(e => e.Salary);
    }
} 