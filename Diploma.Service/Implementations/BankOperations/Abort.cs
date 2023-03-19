using Diploma.Service.Enums;


namespace Diploma.Service.Implementations.BankOperations
{
    public class Abort : Return
    {
        protected override TrType OperationType { get; } = TrType.Abort;
    }
}
