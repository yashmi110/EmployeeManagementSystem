using MediatR;
using DepartmentService.Core.CQRS.Commands;
using DepartmentService.Core.Interfaces;

namespace DepartmentService.Core.CQRS.Handlers;

public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand, bool>
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IEventPublisher _eventPublisher;

    public DeleteDepartmentCommandHandler(IDepartmentRepository departmentRepository, IEventPublisher eventPublisher)
    {
        _departmentRepository = departmentRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<bool> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        var success = await _departmentRepository.DeleteAsync(request.Id);

        if (success)
        {
            // Publish event asynchronously (fire and forget)
            _ = Task.Run(async () =>
            {
                try
                {
                    await _eventPublisher.PublishDepartmentDeletedAsync(request.Id);
                }
                catch (Exception ex)
                {
                    // Log error but don't fail the main operation
                    // In production, you might want to use a more robust error handling strategy
                }
            }, cancellationToken);
        }

        return success;
    }
} 