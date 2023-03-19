using Diploma.Service.Enums;

namespace Diploma.Service.Implementations.BankOperations;

public class EndOfCalculation : Return
{
    protected override TrType OperationType { get; } = TrType.EndOfCalculation;
}