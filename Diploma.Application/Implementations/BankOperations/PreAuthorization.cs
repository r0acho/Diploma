using Diploma.Application.Enums;

namespace Diploma.Application.Implementations.BankOperations;

public class PreAuthorization : Payment
{
    protected override TrType OperationType { get; } = TrType.PreAuthorization;
}