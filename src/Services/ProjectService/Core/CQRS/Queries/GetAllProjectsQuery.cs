using MediatR;
using ProjectService.Core.DTOs;

namespace ProjectService.Core.CQRS.Queries;

public class GetAllProjectsQuery : IRequest<IEnumerable<ProjectDto>>
{
} 