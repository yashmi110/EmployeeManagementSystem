using Shared.Messaging.Contracts;

namespace DepartmentService.Core.Interfaces;

public interface IEventPublisher
{
    Task PublishDepartmentCreatedAsync(int departmentId, string name, string description);
    Task PublishDepartmentUpdatedAsync(int departmentId, string name, string description);
    Task PublishDepartmentDeletedAsync(int departmentId);
} 