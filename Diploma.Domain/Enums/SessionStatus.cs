namespace Diploma.Domain.Enums;

public enum SessionStatus
{
    InProgress,
    ChargingFinished,
    InsufficientFundsError,
    ReconciliationError,
    FiscalizationError,
    Success
}