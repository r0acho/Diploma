using Diploma.Service.Enums;

namespace Diploma.Service.Implementations.BankOperations;

public class RegisterCheck : BankOperationService
{
    protected override TrType OperationType { get; }
    protected override List<string> RequestKeys { get; }
    protected override List<string> PSignOrder { get; }
}