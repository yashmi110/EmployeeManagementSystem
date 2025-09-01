using FluentValidation;
using DepartmentService.Core.CQRS.Commands;

namespace DepartmentService.Core.CQRS.Validators;

public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Department name is required")
            .MaximumLength(100).WithMessage("Department name cannot exceed 100 characters")
            .Matches("^[a-zA-Z0-9\\s\\-]+$").WithMessage("Department name can only contain letters, numbers, spaces, and hyphens");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Department description is required")
            .MaximumLength(500).WithMessage("Department description cannot exceed 500 characters");
    }
} 