using FaultManagement.Domain.Enums;

namespace FaultManagement.Application.DTOs;

public class FaultNotificationDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public PriorityLevel Priority { get; set; }
    public FaultStatus Status { get; set; }
    public int CreatedByUserId { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
}
