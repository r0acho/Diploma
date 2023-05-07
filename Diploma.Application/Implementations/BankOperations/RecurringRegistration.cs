using Diploma.Domain.Entities;
using Diploma.Domain.Enums;

namespace Diploma.Application.Implementations.BankOperations;

public class RecurringRegistration : Payment
{
    public RecurringRegistration(PaymentModel model, byte[] secretKey) : base(model, secretKey)
    {
    }

    protected override TrType OperationType { get; } = TrType.Recurring;

    protected override List<string> RequestKeys { get; } = new()
    {
        "AMOUNT", "CURRENCY", "ORDER", "DESC", "TERMINAL", "TRTYPE", "MERCH_NAME",
        "MERCHANT", "EMAIL", "TIMESTAMP", "NONCE", "BACKREF", "NOTIFY_URL", "RECUR_FREQ", "RECUR_EXP"
    };
}