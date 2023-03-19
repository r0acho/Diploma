using Diploma.Service.Enums;

namespace Diploma.Service.Implementations.BankOperations;

public class Return : BankOperationService
{
    protected override TrType OperationType => TrType.Return;

    protected override List<string> RequestKeys { get; init; } = new()
    {
        "ORDER", "AMOUNT", "CURRENCY", "ORG_AMOUNT", "RRN", "INT_REF", "TRTYPE",
        "TERMINAL", "BACKREF", "EMAIL", "TIMESTAMP", "NONCE", "NOTIFY_URL", "P_SIGN"
    };

    protected override List<string> PSignOrder { get; init; } = new()
    {
        "ORDER", "AMOUNT", "CURRENCY", "ORG_AMOUNT", "RRN",
        "INT_REF", "TRTYPE", "TERMINAL", "BACKREF", "EMAIL", "TIMESTAMP", "NONCE"
    };
}