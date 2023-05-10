using Diploma.Application.Settings;
using Diploma.Domain.Entities;
using Diploma.Domain.Enums;
using Microsoft.Extensions.Options;

namespace Diploma.Application.Implementations.BankOperations;

public class RecurringRegistration : Payment
{
    public RecurringRegistration(PaymentModel model, BankSettings bankSettings) : base(model, bankSettings)
    {
    }

    protected override TrType OperationType { get; } = TrType.Recurring;

    protected override List<string> RequestKeys { get; } = new()
    {
        "AMOUNT", "CURRENCY", "ORDER", "DESC", "TERMINAL", "TRTYPE", "MERCH_NAME",
        "MERCHANT", "EMAIL", "TIMESTAMP", "NONCE", "BACKREF", "NOTIFY_URL", "RECUR_FREQ", "RECUR_EXP"
    };
}