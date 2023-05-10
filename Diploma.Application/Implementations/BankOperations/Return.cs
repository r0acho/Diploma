using Diploma.Application.Settings;
using Diploma.Domain.Entities;
using Diploma.Domain.Enums;
using Microsoft.Extensions.Options;

namespace Diploma.Application.Implementations.BankOperations;

public class Return : BankOperationService
{
    public Return(PaymentModel model, BankSettings bankSettings) : base(model, bankSettings)
    {
    }

    protected override TrType OperationType => TrType.Return;

    protected override List<string> RequestKeys { get; } = new()
    {
        "ORDER", "AMOUNT", "CURRENCY", "ORG_AMOUNT", "RRN", "INT_REF", "TRTYPE",
        "TERMINAL", "BACKREF", "EMAIL", "TIMESTAMP", "NONCE", "NOTIFY_URL", "P_SIGN"
    };

    protected override List<string> PSignOrder { get; } = new()
    {
        "ORDER", "AMOUNT", "CURRENCY", "ORG_AMOUNT", "RRN",
        "INT_REF", "TRTYPE", "TERMINAL", "BACKREF", "EMAIL", "TIMESTAMP", "NONCE"
    };
}