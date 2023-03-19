using Diploma.Service.Enums;

namespace Diploma.Service.Implementations.BankOperations;

public class ReccuringRegistrarion : Payment
{
    protected override TrType OperationType { get; } = TrType.Reccuring;

    protected override List<string> RequestKeys { get; init; } = new()
    {
        "AMOUNT", "CURRENCY", "ORDER", "DESC", "TERMINAL", "TRTYPE", "MERCH_NAME",
        "MERCHANT", "EMAIL", "TIMESTAMP", "NONCE", "BACKREF", "NOTIFY_URL", "RECUR_FREQ", "RECUR_EXP"
    };
}