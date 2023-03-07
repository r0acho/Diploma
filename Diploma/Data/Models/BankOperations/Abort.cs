using Diploma.Data.Enums;
using Diploma.Data.Interfaces;

namespace Diploma.Data.Models.BankOperations
{
    public class Abort : Return
    {
        protected override TrType OperationType { get; } = TrType.Abort;
        
        
    }
}
