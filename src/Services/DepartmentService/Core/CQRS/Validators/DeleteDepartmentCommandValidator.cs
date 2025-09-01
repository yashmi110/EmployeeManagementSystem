using FluentValidation;
using DepartmentService.Core.CQRS.Commands;

namespace DepartmentService.Core.CQRS.Validators;

public class DeleteDepartmentCommandValidator : AbstractValidator<DeleteDepartmentCommand>
{
    public DeleteDepartmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Department ID must be greater than 0");
    }
} 