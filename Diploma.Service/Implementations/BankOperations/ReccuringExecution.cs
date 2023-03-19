using Diploma.Service.Enums;
using Diploma.Service.Interfaces;

namespace Diploma.Service.Implementations.BankOperations
{
    public class ReccuringExecution : Payment
    {
        protected override TrType OperationType { get; } = TrType.Reccuring;

        protected override List<string> RequestKeys { get; init; } = new List<string> {
            "AMOUNT", "CURRENCY", "ORDER", "DESC", "TERMINAL", "TRTYPE", "MERCH_NAME",
            "MERCHANT","EMAIL", "TIMESTAMP","NONCE","BACKREF","NOTIFY_URL", "RECUR_FREQ", "RECUR_EXP"
        };
        
        
    }
}