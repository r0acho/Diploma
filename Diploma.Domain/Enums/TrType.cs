namespace Diploma.Domain.Enums;

/// <summary>
/// Набор кодов платежных операций с банком ПСБ
/// </summary>
public enum TrType
{
    PreAuthorization = 12,
    Abort = 22,
    Pay = 1,
    EndOfCalculation = 21,
    Return = 14,
    Recurring = 171,
    CheckCard = 39
}