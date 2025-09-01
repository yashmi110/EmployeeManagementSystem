using FluentValidation;
using ProjectService.Core.CQRS.Commands;

namespace ProjectService.Core.CQRS.Validators;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required")
            .MaximumLength(100).WithMessage("Project name cannot exceed 100 characters")
            .Matches("^[a-zA-Z0-9\\s\\-]+$").WithMessage("Project name can only contain letters, numbers, spaces, and hyphens");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Project description is required")
            .MaximumLength(500).WithMessage("Project description cannot exceed 500 characters");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required")
            .LessThanOrEqualTo(DateTime.Today.AddYears(1)).WithMessage("Start date cannot be more than 1 year in the future");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).When(x => x.EndDate.HasValue)
            .WithMessage("End date must be after start date");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Project status is required")
            .Must(status => new[] { "Planning", "Active", "On Hold", "Completed", "Cancelled" }.Contains(status))
            .WithMessage("Status must be one of: Planning, Active, On Hold, Completed, Cancelled");

        RuleFor(x => x.Budget)
            .GreaterThan(0).WithMessage("Budget must be greater than 0");

        RuleFor(x => x.ManagerId)
            .GreaterThan(0).WithMessage("Manager ID must be greater than 0");
    }
} 