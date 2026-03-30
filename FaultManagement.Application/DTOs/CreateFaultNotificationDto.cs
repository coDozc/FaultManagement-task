using FaultManagement.Domain.Enums;

namespace FaultManagement.Application.DTOs;

public class CreateFaultNotificationDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public PriorityLevel Priority { get; set; }
}
