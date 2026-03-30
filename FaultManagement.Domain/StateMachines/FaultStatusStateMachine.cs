using FaultManagement.Domain.Enums;

namespace FaultManagement.Domain.StateMachines;

public static class FaultStatusStateMachine
{
    private static readonly Dictionary<FaultStatus, HashSet<FaultStatus>> AllowedTransitions = new()
    {
        [FaultStatus.New] = new() { FaultStatus.UnderReview, FaultStatus.Cancelled },
        [FaultStatus.UnderReview] = new() { FaultStatus.Assigned, FaultStatus.Invalid, FaultStatus.Cancelled },
        [FaultStatus.Assigned] = new() { FaultStatus.InProgress, FaultStatus.Cancelled },
        [FaultStatus.InProgress] = new() { FaultStatus.Completed, FaultStatus.Cancelled },
        [FaultStatus.Completed] = new(),
        [FaultStatus.Cancelled] = new(),
        [FaultStatus.Invalid] = new()
    };

    public static bool CanTransition(FaultStatus from, FaultStatus to)
    {
        return AllowedTransitions.TryGetValue(from, out var targets) && targets.Contains(to);
    }

    public static bool IsTerminal(FaultStatus status)
    {
        return status is FaultStatus.Completed or FaultStatus.Cancelled or FaultStatus.Invalid;
    }
}