# Event-Driven Communication with RabbitMQ and MassTransit

## Overview

This document describes the implementation of event-driven communication between microservices using **RabbitMQ** as the message broker and **MassTransit** as the messaging framework.

## Architecture

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│ Department      │    │ Employee        │    │ Project         │
│ Service         │    │ Service         │    │ Service         │
│                 │    │                 │    │                 │
│ ┌─────────────┐ │    │ ┌─────────────┐ │    │ ┌─────────────┐ │
│ │ Event       │ │    │ │ Event       │ │    │ │ Event       │ │
│ │ Publisher   │ │    │ │ Consumer    │ │    │ │ Consumer    │ │
│ └─────────────┘ │    │ └─────────────┘ │    │ └─────────────┘ │
│         │       │    │                 │    │                 │
│         ▼       │    │                 │    │                 │
└─────────┼───────┘    └─────────────────┘    └─────────────────┘
          │
          ▼
┌─────────────────────────────────────────────────────────────────┐
│                        RabbitMQ                                │
│                                                                 │
│  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐ │
│  │ Department      │  │ Employee        │  │ Project         │ │
│  │ Events Queue    │  │ Events Queue    │  │ Events Queue    │ │
│  └─────────────────┘  └─────────────────┘  └─────────────────┘ │
└─────────────────────────────────────────────────────────────────┘
```

## Components

### 1. Shared Messaging Library (`shared/Messaging`)

Contains all event contracts and message definitions shared across services.

#### Event Contracts

- **BaseMessage**: Common properties for all messages
  - `MessageId`: Unique identifier for each message
  - `Timestamp`: When the message was created
  - `Source`: Which service published the message
  - `CorrelationId`: For tracking related operations

- **Department Events**:
  - `DepartmentCreatedEvent`
  - `DepartmentUpdatedEvent`
  - `DepartmentDeletedEvent`

- **Employee Events**:
  - `EmployeeCreatedEvent`
  - `EmployeeUpdatedEvent`
  - `EmployeeDeletedEvent`
  - `EmployeeDepartmentChangedEvent`

- **Project Events**:
  - `ProjectCreatedEvent`
  - `ProjectUpdatedEvent`
  - `ProjectDeletedEvent`
  - `ProjectStatusChangedEvent`

### 2. Event Publishers

Services that publish events to RabbitMQ when state changes occur.

#### Department Service Event Publisher

```csharp
public interface IEventPublisher
{
    Task PublishDepartmentCreatedAsync(int departmentId, string name, string description);
    Task PublishDepartmentUpdatedAsync(int departmentId, string name, string description);
    Task PublishDepartmentDeletedAsync(int departmentId);
}
```

**Features**:
- Automatic event generation with correlation IDs
- Error handling and logging
- Asynchronous publishing (fire-and-forget)

### 3. Event Consumers

Services that consume events from RabbitMQ and react to changes.

#### Employee Service Consumers

- `DepartmentCreatedEventConsumer`
- `DepartmentUpdatedEventConsumer`
- `DepartmentDeletedEventConsumer`

#### Project Service Consumers

- `DepartmentCreatedEventConsumer`
- `DepartmentUpdatedEventConsumer`
- `DepartmentDeletedEventConsumer`

## Configuration

### MassTransit Setup

Each service configures MassTransit with RabbitMQ:

```csharp
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    // Register consumers
    x.AddConsumer<DepartmentCreatedEventConsumer>();
    x.AddConsumer<DepartmentUpdatedEventConsumer>();
    x.AddConsumer<DepartmentDeletedEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("RabbitMQConnection") ?? "localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});
```

### Connection Strings

```json
{
  "ConnectionStrings": {
    "RabbitMQConnection": "localhost"
  }
}
```

## Event Flow Examples

### 1. Department Creation Flow

```
1. Client POST /api/departments
2. CreateDepartmentCommandHandler processes request
3. Department saved to database
4. DepartmentCreatedEvent published to RabbitMQ
5. Employee Service receives event
6. Project Service receives event
7. Both services update their local caches/data
```

### 2. Department Update Flow

```
1. Client PUT /api/departments/{id}
2. UpdateDepartmentCommandHandler processes request
3. Department updated in database
4. DepartmentUpdatedEvent published to RabbitMQ
5. Employee Service receives event
6. Project Service receives event
7. Both services update their local caches/data
```

### 3. Department Deletion Flow

```
1. Client DELETE /api/departments/{id}
2. DeleteDepartmentCommandHandler processes request
3. Department deleted from database
4. DepartmentDeletedEvent published to RabbitMQ
5. Employee Service receives event
6. Project Service receives event
7. Both services remove from local caches/data
```

## Implementation Details

### Event Publishing in CQRS Handlers

```csharp
public async Task<DepartmentDto> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
{
    var department = await _departmentRepository.CreateAsync(createDepartmentDto);

    // Publish event asynchronously (fire and forget)
    _ = Task.Run(async () =>
    {
        try
        {
            await _eventPublisher.PublishDepartmentCreatedAsync(department.Id, department.Name, department.Description);
        }
        catch (Exception ex)
        {
            // Log error but don't fail the main operation
        }
    }, cancellationToken);

    return department;
}
```

### Event Consumer Implementation

```csharp
public class DepartmentCreatedEventConsumer : IConsumer<DepartmentCreatedEvent>
{
    private readonly ILogger<DepartmentCreatedEventConsumer> _logger;

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
```

## Benefits

### 1. **Loose Coupling**
- Services communicate through events without direct dependencies
- Changes in one service don't affect others
- Easy to add new consumers without modifying publishers

### 2. **Scalability**
- Asynchronous processing allows for better performance
- Multiple consumers can process the same event
- Easy to scale individual services

### 3. **Reliability**
- RabbitMQ provides message persistence
- Automatic retry mechanisms
- Dead letter queues for failed messages

### 4. **Observability**
- Clear event flow tracking
- Correlation IDs for debugging
- Comprehensive logging

## Deployment

### 1. Start RabbitMQ

```bash
docker-compose -f docker-compose.rabbitmq.yml up -d
```

### 2. Verify RabbitMQ Management UI

Access: http://localhost:15672
- Username: `guest`
- Password: `guest`

### 3. Start Services

```bash
.\scripts\run-services.ps1
```

## Monitoring and Debugging

### 1. RabbitMQ Management UI

- **Overview**: Service status and metrics
- **Queues**: Message counts and processing rates
- **Exchanges**: Event routing configuration
- **Connections**: Active service connections

### 2. Logging

Each service logs:
- Event publishing attempts
- Event consumption
- Errors and exceptions
- Performance metrics

### 3. Health Checks

Services include health checks for:
- Database connectivity
- RabbitMQ connectivity
- Cache connectivity

## Best Practices

### 1. **Event Design**
- Keep events small and focused
- Use clear, descriptive names
- Include all necessary data
- Version events when schema changes

### 2. **Error Handling**
- Never fail the main operation due to event publishing
- Implement retry mechanisms
- Use dead letter queues for failed messages
- Log all errors for debugging

### 3. **Performance**
- Use asynchronous publishing
- Implement event batching when possible
- Monitor queue depths and processing rates
- Scale consumers based on load

### 4. **Security**
- Use secure connections (TLS)
- Implement authentication and authorization
- Validate all incoming events
- Monitor for suspicious activity

## Future Enhancements

### 1. **Event Sourcing**
- Store all events in an event store
- Rebuild service state from events
- Implement event replay capabilities

### 2. **Saga Pattern**
- Implement distributed transactions
- Handle complex business workflows
- Compensating actions for failures

### 3. **Event Versioning**
- Support multiple event versions
- Automatic event schema evolution
- Backward compatibility

### 4. **Advanced Routing**
- Content-based routing
- Priority queues
- Dead letter queues
- Message TTL

## Troubleshooting

### Common Issues

1. **Connection Failures**
   - Check RabbitMQ service status
   - Verify connection strings
   - Check network connectivity

2. **Message Loss**
   - Verify message persistence
   - Check consumer health
   - Monitor queue depths

3. **Performance Issues**
   - Monitor queue processing rates
   - Check consumer scaling
   - Review event payload sizes

### Debug Commands

```bash
# Check RabbitMQ status
docker ps | grep rabbitmq

# View RabbitMQ logs
docker logs department-api-rabbitmq

# Check service connectivity
netstat -an | findstr 5672
```

## Conclusion

The event-driven communication implementation provides a robust, scalable foundation for microservices communication. By using RabbitMQ and MassTransit, we achieve:

- **Reliable messaging** with persistence and retry mechanisms
- **Scalable architecture** that can handle high message volumes
- **Loose coupling** between services
- **Comprehensive monitoring** and debugging capabilities

This implementation serves as a solid foundation for building complex, distributed systems that can evolve and scale over time. 