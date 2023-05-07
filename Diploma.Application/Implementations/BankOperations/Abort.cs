using Diploma.Application.Enums;

namespace Diploma.Application.Implementations.BankOperations;

public class Abort : Return
{
    protected override TrType OperationType { get; } = TrType.Abort;
}