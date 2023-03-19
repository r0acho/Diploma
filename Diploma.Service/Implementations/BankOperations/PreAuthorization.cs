using Diploma.Service.Enums;
using Diploma.Service.Interfaces;

namespace Diploma.Service.Implementations.BankOperations
{
    public class PreAuthorization : Payment
    {
        protected override TrType OperationType { get; } = TrType.PreAuthorization;
    }
}
