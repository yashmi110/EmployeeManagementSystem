using Shared.Messaging.Contracts;

namespace Shared.Messaging.Contracts;

public class DepartmentCreatedEvent : BaseMessage
{
    public int DepartmentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class DepartmentUpdatedEvent : BaseMessage
{
    public int DepartmentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class DepartmentDeletedEvent : BaseMessage
{
    public int DepartmentId { get; set; }
} 