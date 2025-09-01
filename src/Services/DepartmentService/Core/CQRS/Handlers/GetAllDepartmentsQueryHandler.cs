using MediatR;
using DepartmentService.Core.CQRS.Queries;
using DepartmentService.Core.DTOs;
using DepartmentService.Core.Interfaces;

namespace DepartmentService.Core.CQRS.Handlers;

public class GetAllDepartmentsQueryHandler : IRequestHandler<GetAllDepartmentsQuery, IEnumerable<DepartmentDto>>
{
    private readonly IDepartmentRepository _departmentRepository;

    public GetAllDepartmentsQueryHandler(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<IEnumerable<DepartmentDto>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
    {
        return await _departmentRepository.GetAllAsync();
    }
} 