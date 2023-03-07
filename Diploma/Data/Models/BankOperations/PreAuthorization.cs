using Diploma.Data.Enums;
using Diploma.Data.Interfaces;

namespace Diploma.Data.Models.BankOperations
{
    public class PreAuthorization : Payment
    {
        protected override TrType OperationType { get; } = TrType.PreAuthorization;
    }
}
