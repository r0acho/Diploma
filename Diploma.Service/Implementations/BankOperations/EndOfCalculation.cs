using Diploma.Service.Enums;
using Diploma.Service.Interfaces;

namespace Diploma.Service.Implementations.BankOperations
{
    public class EndOfCalculation : Return
    {
        protected override TrType OperationType { get; } = TrType.EndOfCalculation;
    }
}
