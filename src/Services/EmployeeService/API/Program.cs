using EmployeeService.Core.Interfaces;
using EmployeeService.Infrastructure;
using EmployeeService.Infrastructure.Repositories;
using EmployeeService.Infrastructure.Consumers;
using Microsoft.EntityFrameworkCore;
using MediatR;
using EmployeeService.Core.CQRS.Commands;
using EmployeeService.Core.CQRS.Queries;
using EmployeeService.Core.CQRS.Validators;
using Shared.Caching;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Configure Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Caching
builder.Services.AddMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection") ?? "localhost:6379";
    options.InstanceName = "EmployeeService_";
});

// Register cache services
builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddScoped<ICacheService, MemoryCacheService>();

// Register services
builder.Services.AddScoped<IEmployeeService, EmployeeService.Infrastructure.Repositories.EmployeeService>();

// Configure MassTransit
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

// Register MediatR
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(CreateEmployeeCommand).Assembly);
});

// Register FluentValidation (temporarily disabled due to package version issues)
// builder.Services.AddValidatorsFromAssemblyContaining<CreateEmployeeCommandValidator>();
// builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
