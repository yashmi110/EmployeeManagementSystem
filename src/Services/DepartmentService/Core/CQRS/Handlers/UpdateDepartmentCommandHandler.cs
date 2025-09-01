using MediatR;
using DepartmentService.Core.CQRS.Commands;
using DepartmentService.Core.DTOs;
using DepartmentService.Core.Interfaces;

namespace DepartmentService.Core.CQRS.Handlers;

public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, DepartmentDto?>
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IEventPublisher _eventPublisher;

    public UpdateDepartmentCommandHandler(IDepartmentRepository departmentRepository, IEventPublisher eventPublisher)
    {
        _departmentRepository = departmentRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<DepartmentDto?> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var updateDepartmentDto = new UpdateDepartmentDto
        {
            Name = request.Name,
            Description = request.Description
        };

        var department = await _departmentRepository.UpdateAsync(request.Id, updateDepartmentDto);

        if (department != null)
        {
            // Publish event asynchronously (fire and forget)
            _ = Task.Run(async () =>
            {
                try
                {
                    await _eventPublisher.PublishDepartmentUpdatedAsync(department.Id, department.Name, department.Description);
                }
                catch (Exception ex)
                {
                    // Log error but don't fail the main operation
                    // In production, you might want to use a more robust error handling strategy
                }
            }, cancellationToken);
        }

        return department;
    }
} 