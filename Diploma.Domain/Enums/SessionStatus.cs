namespace Diploma.Domain.Enums;

/// <summary>
/// Перечисление состояния сессий
/// </summary>
public enum SessionStatus
{
    InProgress,
    InsufficientFundsError,
    ReconciliationError,
    FiscalizationError,
    Success
}