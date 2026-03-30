using FaultManagement.Domain.Enums;

namespace FaultManagement.Application.DTOs;

public class ChangeFaultStatusDto
{
    public FaultStatus NewStatus { get; set; }
}
