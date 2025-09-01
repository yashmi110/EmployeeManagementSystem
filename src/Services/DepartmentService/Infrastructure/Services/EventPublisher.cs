using DepartmentService.Core.Interfaces;
using Shared.Messaging.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace DepartmentService.Infrastructure.Services;

public class EventPublisher : IEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<EventPublisher> _logger;

    public EventPublisher(IPublishEndpoint publishEndpoint, ILogger<EventPublisher> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task PublishDepartmentCreatedAsync(int departmentId, string name, string description)
    {
        try
        {
            var @event = new DepartmentCreatedEvent
            {
                DepartmentId = departmentId,
                Name = name,
                Description = description,
                Source = "DepartmentService",
                CorrelationId = Guid.NewGuid().ToString()
            };

            await _publishEndpoint.Publish(@event);
            _logger.LogInformation("Published DepartmentCreatedEvent for department {DepartmentId}", departmentId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish DepartmentCreatedEvent for department {DepartmentId}", departmentId);
            throw;
        }
    }

    public async Task PublishDepartmentUpdatedAsync(int departmentId, string name, string description)
    {
        try
        {
            var @event = new DepartmentUpdatedEvent
            {
                DepartmentId = departmentId,
                Name = name,
                Description = description,
                Source = "DepartmentService",
                CorrelationId = Guid.NewGuid().ToString()
            };

            await _publishEndpoint.Publish(@event);
            _logger.LogInformation("Published DepartmentUpdatedEvent for department {DepartmentId}", departmentId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish DepartmentUpdatedEvent for department {DepartmentId}", departmentId);
            throw;
        }
    }

    public async Task PublishDepartmentDeletedAsync(int departmentId)
    {
        try
        {
            var @event = new DepartmentDeletedEvent
            {
                DepartmentId = departmentId,
                Source = "DepartmentService",
                CorrelationId = Guid.NewGuid().ToString()
            };

            await _publishEndpoint.Publish(@event);
            _logger.LogInformation("Published DepartmentDeletedEvent for department {DepartmentId}", departmentId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish DepartmentDeletedEvent for department {DepartmentId}", departmentId);
            throw;
        }
    }
} 