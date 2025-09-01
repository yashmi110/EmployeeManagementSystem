using Shared.Messaging.Contracts;

namespace Shared.Messaging.Contracts;

public class ProjectCreatedEvent : BaseMessage
{
    public int ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal Budget { get; set; }
    public int ManagerId { get; set; }
}

public class ProjectUpdatedEvent : BaseMessage
{
    public int ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal Budget { get; set; }
    public int ManagerId { get; set; }
}

public class ProjectDeletedEvent : BaseMessage
{
    public int ProjectId { get; set; }
}

public class ProjectStatusChangedEvent : BaseMessage
{
    public int ProjectId { get; set; }
    public string OldStatus { get; set; } = string.Empty;
    public string NewStatus { get; set; } = string.Empty;
} 