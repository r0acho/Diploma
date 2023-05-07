using Diploma.Application.Enums;

namespace Diploma.Application.Implementations.BankOperations;

public class EndOfCalculation : Return
{
    protected override TrType OperationType { get; } = TrType.EndOfCalculation;
}