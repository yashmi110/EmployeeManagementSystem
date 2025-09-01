using FluentValidation;
using EmployeeService.Core.CQRS.Commands;

namespace EmployeeService.Core.CQRS.Validators;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Employee name is required")
            .MaximumLength(100).WithMessage("Employee name cannot exceed 100 characters")
            .Matches("^[a-zA-Z\\s]+$").WithMessage("Employee name can only contain letters and spaces");

        RuleFor(x => x.Age)
            .GreaterThan(0).WithMessage("Age must be greater than 0")
            .LessThanOrEqualTo(120).WithMessage("Age cannot exceed 120");

        RuleFor(x => x.Department)
            .NotEmpty().WithMessage("Department is required")
            .MaximumLength(100).WithMessage("Department name cannot exceed 100 characters");

        RuleFor(x => x.Salary)
            .GreaterThan(0).WithMessage("Salary must be greater than 0");

        RuleFor(x => x.HireDate)
            .NotEmpty().WithMessage("Hire date is required")
            .LessThanOrEqualTo(DateTime.Today).WithMessage("Hire date cannot be in the future");

        // Manager-specific validation
        When(x => !string.IsNullOrEmpty(x.ManagementLevel), () =>
        {
            RuleFor(x => x.ManagementLevel)
                .MaximumLength(50).WithMessage("Management level cannot exceed 50 characters");

            RuleFor(x => x.TeamSize)
                .GreaterThanOrEqualTo(0).WithMessage("Team size cannot be negative");
        });
    }
} 