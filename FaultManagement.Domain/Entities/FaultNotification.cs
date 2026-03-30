using FaultManagement.Domain.Enums;
using FaultManagement.Domain.Exceptions;
using FaultManagement.Domain.StateMachines;

namespace FaultManagement.Domain.Entities;

public class FaultNotification
{
    public Guid Id { get; init; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Location { get; private set; } = string.Empty;
    public PriorityLevel Priority { get; private set; }
    public FaultStatus Status { get; private set; }
    public Guid CreatedByUserId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }

    private FaultNotification()
    {
    }

    public FaultNotification(
        string title,
        string description,
        string location,
        PriorityLevel priority,
        Guid createdByUserId)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        Location = location;
        Priority = priority;
        Status = FaultStatus.New;
        CreatedByUserId = createdByUserId;
        CreatedAtUtc = DateTime.UtcNow;
    }

    // Constructor for seeding with specific ID and Status
    public FaultNotification(
        Guid id,
        string title,
        string description,
        string location,
        PriorityLevel priority,
        FaultStatus status,
        Guid createdByUserId)
    {
        Id = id;
        Title = title;
        Description = description;
        Location = location;
        Priority = priority;
        Status = status;
        CreatedByUserId = createdByUserId;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void ChangeStatus(FaultStatus newStatus)
    {
        if (Status == newStatus)
        {
            throw new InvalidStatusTransitionException("Status is already this value.");
        }

        if (!FaultStatusStateMachine.CanTransition(Status, newStatus))
        {
            throw new InvalidStatusTransitionException(
                $"Invalid transition: {Status} -> {newStatus}");
        }

        Status = newStatus;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
