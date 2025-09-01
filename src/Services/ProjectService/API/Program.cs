using ProjectService.Core.Interfaces;
using ProjectService.Infrastructure;
using ProjectService.Infrastructure.Repositories;
using ProjectService.Infrastructure.Consumers;
using Microsoft.EntityFrameworkCore;
using MediatR;
using FluentValidation;
using ProjectService.Core.CQRS.Commands;
using ProjectService.Core.CQRS.Queries;
using ProjectService.Core.CQRS.Validators;
using Shared.Caching;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
    options.InstanceName = "ProjectService_";
});

// Register cache services
builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddScoped<ICacheService, MemoryCacheService>();

// Register services
builder.Services.AddScoped<IProjectService, ProjectService.Infrastructure.Repositories.ProjectService>();

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
    cfg.RegisterServicesFromAssembly(typeof(CreateProjectCommand).Assembly);
});

// Register FluentValidation (temporarily disabled due to package version issues)
// builder.Services.AddValidatorsFromAssemblyContaining<CreateProjectCommandValidator>();
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
