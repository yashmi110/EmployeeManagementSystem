using FluentValidation;
using ProjectService.Core.CQRS.Commands;

namespace ProjectService.Core.CQRS.Validators;

public class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
{
    public DeleteProjectCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Project ID must be greater than 0");
    }
} 