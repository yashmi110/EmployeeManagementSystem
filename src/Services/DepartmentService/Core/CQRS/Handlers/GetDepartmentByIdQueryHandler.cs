using MediatR;
using DepartmentService.Core.CQRS.Queries;
using DepartmentService.Core.DTOs;
using DepartmentService.Core.Interfaces;

namespace DepartmentService.Core.CQRS.Handlers;

public class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentByIdQuery, DepartmentDto?>
{
    private readonly IDepartmentRepository _departmentRepository;

    public GetDepartmentByIdQueryHandler(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<DepartmentDto?> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        return await _departmentRepository.GetByIdAsync(request.Id);
    }
} 