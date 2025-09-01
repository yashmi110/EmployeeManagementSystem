using DepartmentService.Core.Interfaces;
using DepartmentService.Infrastructure;
using DepartmentService.Infrastructure.Repositories;
using DepartmentService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using MediatR;
using DepartmentService.Core.CQRS.Commands;
using DepartmentService.Core.CQRS.Queries;
using DepartmentService.Core.CQRS.Validators;
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
    options.InstanceName = "DepartmentService_";
});

// Register cache services
builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddScoped<ICacheService, MemoryCacheService>();

// Register repositories
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

// Register event publisher
builder.Services.AddScoped<IEventPublisher, EventPublisher>();

// Configure MassTransit
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

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
    cfg.RegisterServicesFromAssembly(typeof(CreateDepartmentCommand).Assembly);
});

// Register FluentValidation (temporarily disabled due to package version issues)
// builder.Services.AddValidatorsFromAssemblyContaining<CreateDepartmentCommandValidator>();
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
