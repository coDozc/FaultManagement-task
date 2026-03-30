using FaultManagement.Domain.Enums;

namespace FaultManagement.Application.DTOs;

public class ListFaultsQueryDto
{
    public int? Page { get; set; } = 1;
    public int? PageSize { get; set; } = 10;
    public FaultStatus? Status { get; set; }
    public PriorityLevel? Priority { get; set; }
    public string? LocationFilter { get; set; }
    public string? SortBy { get; set; } = "createdAt"; // "priority" or "createdAt"
}
