using Diploma.Service.Enums;

namespace Diploma.Service.Implementations.BankOperations;

public class PreAuthorization : Payment
{
    protected override TrType OperationType { get; } = TrType.PreAuthorization;
}