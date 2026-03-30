namespace FaultManagement.Domain.Exceptions;

public sealed class InvalidStatusTransitionException : Exception
{
    public InvalidStatusTransitionException(string message) : base(message)
    {
    }
}
