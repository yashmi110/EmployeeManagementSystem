using MassTransit;
using Shared.Messaging.Contracts;
using Microsoft.Extensions.Logging;

namespace ProjectService.Infrastructure.Consumers;

public class DepartmentCreatedEventConsumer : IConsumer<DepartmentCreatedEvent>
{
    private readonly ILogger<DepartmentCreatedEventConsumer> _logger;

    public DepartmentCreatedEventConsumer(ILogger<DepartmentCreatedEventConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DepartmentCreatedEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Received DepartmentCreatedEvent: Department {DepartmentId} - {Name}", 
            message.DepartmentId, message.Name);

        // Here you would typically:
        // 1. Update local cache
        // 2. Update local database if needed
        // 3. Trigger any business logic
        // 4. Send notifications

        await Task.CompletedTask;
    }
}

public class DepartmentUpdatedEventConsumer : IConsumer<DepartmentUpdatedEvent>
{
    private readonly ILogger<DepartmentUpdatedEventConsumer> _logger;

    public DepartmentUpdatedEventConsumer(ILogger<DepartmentUpdatedEventConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DepartmentUpdatedEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Received DepartmentUpdatedEvent: Department {DepartmentId} - {Name}", 
            message.DepartmentId, message.Name);

        // Here you would typically:
        // 1. Update local cache
        // 2. Update local database if needed
        // 3. Trigger any business logic
        // 4. Send notifications

        await Task.CompletedTask;
    }
}

public class DepartmentDeletedEventConsumer : IConsumer<DepartmentDeletedEvent>
{
    private readonly ILogger<DepartmentDeletedEventConsumer> _logger;

    public DepartmentDeletedEventConsumer(ILogger<DepartmentDeletedEventConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DepartmentDeletedEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Received DepartmentDeletedEvent: Department {DepartmentId}", 
            message.DepartmentId);

        // Here you would typically:
        // 1. Remove from local cache
        // 2. Update local database if needed
        // 3. Trigger any business logic
        // 4. Send notifications

        await Task.CompletedTask;
    }
} 