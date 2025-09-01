namespace Shared.Messaging.Contracts;

public abstract class BaseMessage
{
    public Guid MessageId { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Source { get; set; } = string.Empty;
    public string CorrelationId { get; set; } = string.Empty;
} 