using FluentValidation;
using EmployeeService.Core.CQRS.Commands;

namespace EmployeeService.Core.CQRS.Validators;

public class DeleteEmployeeCommandValidator : AbstractValidator<DeleteEmployeeCommand>
{
    public DeleteEmployeeCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Employee ID must be greater than 0");
    }
} 