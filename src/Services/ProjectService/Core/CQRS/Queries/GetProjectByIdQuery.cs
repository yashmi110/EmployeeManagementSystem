using MediatR;
using ProjectService.Core.DTOs;

namespace ProjectService.Core.CQRS.Queries;

public class GetProjectByIdQuery : IRequest<ProjectDto?>
{
    public int Id { get; set; }
} 